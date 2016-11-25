#region References

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using TestR.Desktop;
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
			Monitor();
		}

		private static void Monitor()
		{
			Element lastAutoFocusedElement = null;
			Element lastCtrlFocusedElement = null;
			Application application = null;

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
							Console.WriteLine("?" + foundElement.ApplicationId);

							lastAutoFocusedElement = foundElement;
							var element = application.Get(foundElement.ApplicationId, wait: false);
							if (element != null)
							{
								Console.WriteLine("+" + element.ApplicationId);
								//Console.WriteLine(element.Parent.FullId);
								//Console.WriteLine(element.NativeElement.CurrentNativeWindowHandle);
							}
						}
					}

					if (!Keyboard.IsControlPressed())
					{
						Thread.Sleep(150);
						continue;
					}

					foundElement = Element.FromCursor();

					if (foundElement?.ProcessId == application.Process.Id)
					{
						foundElement?.UpdateParents();

						if (foundElement?.ApplicationId != lastCtrlFocusedElement?.ApplicationId)
						{
							Console.WriteLine("?" + foundElement.ApplicationId);

							lastCtrlFocusedElement = foundElement;
							var element = application.Get(foundElement.ApplicationId, wait: false);
							if (element != null)
							{
								Console.WriteLine("+" + element.ApplicationId);
								//Console.WriteLine(element.Parent.FullId);
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

		#endregion
	}
}