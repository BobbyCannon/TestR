#region References

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using TestR.Desktop;
using TestR.Extensions;
using TestR.Native;

#endregion

namespace TestR.Sample
{
	public static class Program
	{
		#region Methods

		[STAThread]
		private static void Main(string[] args)
		{
            Console.WriteLine("Is64 Process: " + Environment.Is64BitProcess);
			Monitor();
			//Monitor2();
		}

		private static void Monitor()
		{
			Element lastAutoFocusedElement = null;
			Element lastCtrlFocusedElement = null;
			Application application = null;

			while (!Console.KeyAvailable)
			{
				System.Windows.Forms.Application.DoEvents();

				try
				{
					Element foundElement;

					if (Keyboard.IsControlPressed() && application == null)
					{
						foundElement = Element.FromCursor();
						var process = Process.GetProcessById(foundElement.ProcessId);
						application = Application.Attach(process);
						Console.WriteLine("Attached to " + process.Id);
						//application.Children.PrintDebug(verbose: false);
						//Thread.Sleep(25);
						continue;
					}

					if (application == null)
					{
						continue;
					}

					foundElement = Element.FromFocusElement();

					if (foundElement?.ProcessId == application.Process.Id)
					{
						Debug.WriteLine("Updating Parents...");
						foundElement?.UpdateParents();

						if (foundElement?.ApplicationId != lastAutoFocusedElement?.ApplicationId)
						{
							Console.WriteLine(foundElement.ApplicationId);

							lastAutoFocusedElement = foundElement;
							var element = application.Get(foundElement.ApplicationId, wait: false);
							if (element != null)
							{
								Console.WriteLine(element.ApplicationId);
								//Console.WriteLine(element.Parent.ApplicationId);
								//Console.WriteLine(element.NativeElement.CurrentNativeWindowHandle);
							}
						}
					}

					if (!Keyboard.IsControlPressed())
					{
						Thread.Sleep(2000);
						continue;
					}

					foundElement = Element.FromCursor();

					if (foundElement?.ProcessId == application.Process.Id)
					{
						foundElement?.UpdateParents();

						if (foundElement?.ApplicationId != lastCtrlFocusedElement?.ApplicationId)
						{
							Console.WriteLine(foundElement.ApplicationId);

							lastCtrlFocusedElement = foundElement;
							var element = application.Get(foundElement.ApplicationId, wait: false);
							if (element != null)
							{
								Console.WriteLine(element.ApplicationId);
								//Console.WriteLine(element.Parent.ApplicationId);
								//Console.WriteLine(element.NativeElement.CurrentNativeWindowHandle);
							}
						}
					}

					Thread.Sleep(25);
				}
				catch (COMException)
				{
					// We have lost access to the application.
					Console.WriteLine("Lost access to the application...");
					lastAutoFocusedElement = null;
					lastCtrlFocusedElement = null;
					application.Dispose();
					application = null;
				}
				catch (Exception ex)
				{
					Console.WriteLine("Failed to get element... " + ex.Message);
				}
			}

			application?.Dispose();
		}

		private static void Monitor2()
		{
			Element lastAutoFocusedElement = null;
			Element lastCtrlFocusedElement = null;
			Application application = null;
			var watch = Stopwatch.StartNew();

			while (!Console.KeyAvailable)
			{
				try
				{
					Element foundElement;

					if (Keyboard.IsControlPressed() && application == null)
					{
						foundElement = Element.FromCursor();
						var process = Process.GetProcessById(foundElement.ProcessId);
						application = Application.Attach(process);
						Console.WriteLine("Attached to " + process.Id);
						application.Children.PrintDebug(verbose: false);
						Thread.Sleep(1000);
						continue;
					}

					if (application == null)
					{
						continue;
					}

					foundElement = Element.FromFocusElement();

					if (foundElement?.ProcessId == application.Process.Id)
					{
						foundElement?.UpdateParents();

						if (foundElement != null && foundElement.ProcessId == application.Process.Id && foundElement.ApplicationId != lastAutoFocusedElement?.ApplicationId)
						{
							lastAutoFocusedElement = foundElement;
							Console.WriteLine("\n\n\nAUTO: " + foundElement.ApplicationId);
							watch.Restart();
							var details = foundElement.ToDetailString();
							Console.WriteLine("{0}:{1}", watch.Elapsed.ToString("ss\\:fff"), details);
							Console.WriteLine("Parents:");
							foundElement.Parents.ForEach(x => Console.WriteLine("\t" + x.ApplicationId));
							Console.WriteLine("Children:");
							foundElement.Children.ForEach(x => Console.WriteLine("\t" + x.ApplicationId));
						}
					}

					if (!Keyboard.IsControlPressed())
					{
						Thread.Sleep(100);
						continue;
					}

					foundElement = Element.FromCursor();

					if (foundElement?.ProcessId == application.Process.Id)
					{
						foundElement?.UpdateParents();

						if (foundElement == lastAutoFocusedElement)
						{
							Console.WriteLine("Skipping....");
							continue;
						}

						if (foundElement != null && foundElement.ProcessId == application.Process.Id && foundElement.ApplicationId != lastCtrlFocusedElement?.ApplicationId)
						{
							lastCtrlFocusedElement = foundElement;
							Console.WriteLine("CTRL: " + foundElement.ApplicationId);
							watch.Restart();
							foundElement.UpdateChildren();
							var details = foundElement.ToDetailString();
							Console.WriteLine("{0}:{1}", watch.Elapsed.ToString("ss\\:fff"), details);

							foreach (var child in foundElement.Children)
							{
								Console.WriteLine("\t" + child.ApplicationId);
							}
						}
					}

					Thread.Sleep(100);
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Failed to get element... " + ex.Message);
				}
			}

			application?.Dispose();
		}

		#endregion
	}
}