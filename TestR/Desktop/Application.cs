#region References

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using TestR.Desktop.Automation;
using TestR.Desktop.Elements;
using TestR.Extensions;
using TestR.Helpers;
using TestR.Native;
using Utility = TestR.Helpers.Utility;

#endregion

namespace TestR.Desktop
{
	/// <summary>
	/// Represents an application that can be automated.
	/// </summary>
	public class Application : IDisposable, IElementParent
	{
		#region Constructors

		/// <summary>
		/// Creates an instance of the application.
		/// </summary>
		/// <param name="process"> The process for the application. </param>
		internal Application(Process process)
		{
			Children = new ElementCollection<Element>(this);
			Process = process;
			Timeout = TimeSpan.FromSeconds(5);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a flag to auto close the application when disposed of.
		/// </summary>
		public bool AutoClose { get; set; }

		/// <summary>
		/// Gets the children for this element.
		/// </summary>
		public ElementCollection<Element> Children { get; private set; }

		/// <summary>
		/// Gets the handle for this window.
		/// </summary>
		public IntPtr Handle
		{
			get { return Process.MainWindowHandle; }
		}

		/// <summary>
		/// Gets the ID of this application.
		/// </summary>
		public string Id
		{
			get { return Process.Id.ToString(); }
		}

		/// <summary>
		/// Gets the value indicating that the process is running.
		/// </summary>
		public bool IsRunning
		{
			get { return Process != null && !Process.HasExited; }
		}

		/// <summary>
		/// Gets the name of this element.
		/// </summary>
		public string Name
		{
			get { return Handle.ToString(); }
		}

		/// <summary>
		/// Gets the underlying process for this application.
		/// </summary>
		public Process Process { get; private set; }

		/// <summary>
		/// Gets or sets the time out for delay request. Defaults to 5 seconds.
		/// </summary>
		public TimeSpan Timeout { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Attaches the application to an existing process.
		/// </summary>
		/// <param name="executablePath"> The path to the executable. </param>
		/// <param name="arguments"> The arguments for the executable. Arguments are optional. </param>
		/// <returns> The instance that represents the application. </returns>
		public static Application Attach(string executablePath, string arguments = null)
		{
			var fileName = Path.GetFileName(executablePath);
			if (fileName != null && !fileName.Contains("."))
			{
				fileName += ".exe";
			}

			var processName = Path.GetFileNameWithoutExtension(executablePath);
			var query = string.Format("SELECT Handle, CommandLine FROM Win32_Process WHERE Name='{0}'", fileName);

			using (var searcher = new ManagementObjectSearcher(query))
			{
				foreach (var result in searcher.Get())
				{
					var managementObject = (ManagementObject) result;
					var handle = int.Parse(managementObject["Handle"].ToString());
					if (!string.IsNullOrWhiteSpace(arguments))
					{
						var data = managementObject["CommandLine"];
						if (data == null || !data.ToString().Contains(arguments))
						{
							continue;
						}
					}

					var process = Process.GetProcessesByName(processName).FirstOrDefault(x => x.Id == handle);
					if (process == null)
					{
						continue;
					}

					if (process.MainWindowHandle == IntPtr.Zero || !NativeMethods.IsWindowVisible(process.MainWindowHandle))
					{
						continue;
					}

					var application = new Application(process);
					application.Refresh();
					application.WaitWhileBusy();

					return application;
				}
			}

			return null;
		}

		/// <summary>
		/// Attaches the application to an existing process.
		/// </summary>
		/// <param name="handle"> The handle of the executable. </param>
		/// <returns> The instance that represents the application. </returns>
		public static Application Attach(IntPtr handle)
		{
			var process = Process.GetProcesses().FirstOrDefault(x => x.MainWindowHandle == handle);
			if (process == null)
			{
				return null;
			}

			var application = new Application(process);
			application.Refresh();
			application.WaitWhileBusy();

			return application;
		}

		/// <summary>
		/// Attaches the application to an existing process using the title.
		/// </summary>
		/// <param name="title"> The title of the window in the application. </param>
		/// <returns> The instance that represents the application. </returns>
		public static Application AttachByTitle(string title)
		{
			var window = ElementWalker.GetChildren(AutomationElement.RootElement)
				.Where(x => x.Current.ControlType.ProgrammaticName == ControlType.Window.ProgrammaticName)
				.FirstOrDefault(x => x.Current.Name.Equals(title, StringComparison.OrdinalIgnoreCase));

			if (window == null)
			{
				return null;
			}

			var application = new Application(Process.GetProcessById(window.Current.ProcessId));
			application.Refresh();
			application.WaitWhileBusy();

			return application;
		}

		/// <summary>
		/// Attaches the application to an existing process.
		/// </summary>
		/// <param name="executablePath"> The path to the executable. </param>
		/// <param name="arguments"> The arguments for the executable. Arguments are optional. </param>
		/// <returns> The instance that represents the application. </returns>
		public static Application AttachOrCreate(string executablePath, string arguments = null)
		{
			return Attach(executablePath, arguments) ?? Create(executablePath, arguments);
		}

		/// <summary>
		/// Brings the application to the front and makes it the Top window.
		/// </summary>
		public void BringToFront()
		{
			NativeMethods.SetForegroundWindow(Handle);
			NativeMethods.BringWindowToTop(Handle);
		}

		/// <summary>
		/// Closes the window.
		/// </summary>
		public void Close()
		{
			if (Process.HasExited)
			{
				return;
			}

			Process.CloseMainWindow();
			if (!Process.WaitForExit(1500))
			{
				Process.Kill();
			}
		}

		/// <summary>
		/// Closes all windows my name and closes them.
		/// </summary>
		/// <param name="executablePath"> The path to the executable. </param>
		public static void CloseAll(string executablePath)
		{
			var processName = Path.GetFileNameWithoutExtension(executablePath);

			// Find all the main processes.
			var processes = Process.GetProcessesByName(processName)
				.Where(x => x.MainWindowHandle != IntPtr.Zero);

			processes.ForEachDisposable(process =>
			{
				// Ask to close the process nicely.
				process.CloseMainWindow();

				if (!process.WaitForExit(1000))
				{
					// The process did not close so now we are just going to kill it.
					process.Kill();
					process.WaitForExit();
				}
			});

			// Wait for the threads to sleep and child process to close.
			Thread.Sleep(50);

			// Find all the other processes.
			Process.GetProcessesByName(processName).ForEachDisposable(process =>
			{
				// The process did not close so now we are just going to kill it.
				process.Kill();
				process.WaitForExit();
			});
		}

		/// <summary>
		/// Creates a new application process.
		/// </summary>
		/// <param name="executablePath"> The path to the executable. </param>
		/// <param name="arguments"> The arguments for the executable. Arguments are optional. </param>
		/// <param name="refresh"> The flag to trigger loading to load state when creating the application. Defaults to true. </param>
		/// <returns> The instance that represents an application. </returns>
		public static Application Create(string executablePath, string arguments = null, bool refresh = true)
		{
			var processStartInfo = new ProcessStartInfo(executablePath);
			if (!string.IsNullOrWhiteSpace(arguments))
			{
				processStartInfo.Arguments = arguments;
			}

			var process = Process.Start(processStartInfo);
			if (process == null)
			{
				throw new InvalidOperationException("Failed to start the application.");
			}

			process.WaitForInputIdle();
			var application = new Application(process);

			if (refresh)
			{
				application.Refresh();
				application.WaitWhileBusy();
			}

			return application;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Checks to see if an application process exist by path and optional arguments.
		/// </summary>
		/// <param name="executablePath"> The path to the executable. </param>
		/// <param name="arguments"> The arguments for the executable. Arguments are optional. </param>
		/// <returns> True if the application exists and false otherwise. </returns>
		public static bool Exists(string executablePath, string arguments = null)
		{
			var fileName = Path.GetFileName(executablePath);
			var processName = Path.GetFileNameWithoutExtension(executablePath);
			var query = string.Format("SELECT Handle, CommandLine FROM Win32_Process WHERE Name='{0}'", fileName);

			using (var searcher = new ManagementObjectSearcher(query))
			{
				foreach (var result in searcher.Get())
				{
					var managementObject = (ManagementObject) result;
					var handle = int.Parse(managementObject["Handle"].ToString());
					if (!string.IsNullOrWhiteSpace(arguments))
					{
						var data = managementObject["CommandLine"];
						if (data == null || !data.ToString().Contains(arguments))
						{
							continue;
						}
					}

					using (var process = Process.GetProcessesByName(processName).FirstOrDefault(x => x.Id == handle))
					{
						if (process == null)
						{
							continue;
						}

						if (process.MainWindowHandle == IntPtr.Zero || !NativeMethods.IsWindowVisible(process.MainWindowHandle))
						{
							continue;
						}

						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Get a child of a certain type and key.
		/// </summary>
		/// <typeparam name="T"> The type of the child. </typeparam>
		/// <param name="key"> The key of the child. </param>
		/// <param name="includeDescendance"> Flag to determine to include descendance or not. </param>
		/// <returns> The child if found or null if otherwise. </returns>
		public T GetChild<T>(string key, bool includeDescendance) where T : Element, IElementParent
		{
			return (T) Children.GetChild(key, includeDescendance);
		}

		/// <summary>
		/// Returns a value indicating if the windows is in front of all other windows.
		/// </summary>
		/// <returns> </returns>
		public bool IsInFront()
		{
			var handle = NativeMethods.GetForegroundWindow();
			return handle == Process.MainWindowHandle;
		}

		/// <summary>
		/// Move the window and resize it.
		/// </summary>
		/// <param name="x"> The x coordinate to move to. </param>
		/// <param name="y"> The y coordinate to move to. </param>
		/// <param name="width"> The width of the window. </param>
		/// <param name="height"> The height of the window. </param>
		public void MoveWindow(int x, int y, int width, int height)
		{
			NativeMethods.MoveWindow(Handle, x, y, width, height, true);
		}

		/// <summary>
		/// Refresh the list of items for the application.
		/// </summary>
		public void Refresh()
		{
			try
			{
				Utility.Wait(() =>
				{
					var windows = ElementWalker.GetWindowsForProcess(Process.Id)
						.Select(x => new Window(x, this))
						.ToList();

					Children.Clear();
					Children.AddRange(windows);

					return Children.Any();
				}, Timeout.TotalMilliseconds, 100);

				WaitWhileBusy();

				Children.ForEach(x => x.UpdateChildren());

				WaitWhileBusy();
			}
			catch (ElementNotAvailableException)
			{
				// A window close while trying to enumerate it. Wait for a second then try again.
				Thread.Sleep(1000);
				Refresh();
			}
		}

		/// <summary>
		/// Update the children for this element.
		/// </summary>
		public void UpdateChildren()
		{
			Refresh();
			OnChildrenUpdated();
		}

		/// <summary>
		/// Wait for the child to be available then return it.
		/// </summary>
		/// <param name="id"> The ID of the child to wait for. </param>
		/// <param name="includeDescendance"> Flag to determine to include descendance or not. </param>
		/// <returns> The child element for the ID. </returns>
		public Element WaitForChild(string id, bool includeDescendance = true)
		{
			return WaitForChild<Element>(id, includeDescendance);
		}

		/// <summary>
		/// Wait for the child to be available then return it.
		/// </summary>
		/// <param name="id"> The ID of the child to wait for. </param>
		/// <param name="includeDescendance"> Flag to determine to include descendance or not. </param>
		/// <returns> The child element for the ID. </returns>
		public T WaitForChild<T>(string id, bool includeDescendance = true) where T : Element
		{
			Element response = null;

			Utility.Wait(() =>
			{
				response = GetChild<T>(id, includeDescendance);
				if (response != null)
				{
					return true;
				}

				UpdateChildren();
				return false;
			}, Timeout.TotalMilliseconds, 100);

			if (response == null)
			{
				throw new ArgumentException("Failed to find the child by ID.");
			}

			return (T) response;
		}

		/// <summary>
		/// Waits for the Process to not be busy.
		/// </summary>
		/// <param name="minimumDelay"> The minimum delay in milliseconds to wait. Defaults to 0 milliseconds. </param>
		public void WaitWhileBusy(int minimumDelay = 0)
		{
			var watch = Stopwatch.StartNew();
			Process.WaitForInputIdle(Timeout.Milliseconds);

			while (watch.Elapsed.TotalMilliseconds < minimumDelay && minimumDelay > 0)
			{
				Thread.Sleep(1);
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing"> True if disposing and false if otherwise. </param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}

			if (AutoClose && Process != null && Process.HasExited)
			{
				Close();
			}

			if (Process != null)
			{
				Process.Dispose();
				Process = null;
			}
		}

		/// <summary>
		/// Handles the children updated event.
		/// </summary>
		protected virtual void OnChildrenUpdated()
		{
			var handler = ChildrenUpdated;
			if (handler != null)
			{
				handler();
			}
		}

		/// <summary>
		/// Handles the excited event.
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="e"> </param>
		private void OnExited(object sender, EventArgs e)
		{
			var exited = Exited;
			if (exited != null)
			{
				exited();
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the children are updated.
		/// </summary>
		public event Action ChildrenUpdated;

		/// <summary>
		/// Occurs when the application exits.
		/// </summary>
		public event Action Exited;

		#endregion
	}
}