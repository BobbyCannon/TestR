#region References

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using TestR.Desktop;
using TestR.Native;

#endregion

namespace TestR
{
	/// <summary>
	/// Represents an application that can be automated.
	/// </summary>
	public class Application : ElementHost
	{
		#region Constants

		/// <summary>
		/// Gets the default timeout (in milliseconds).
		/// </summary>
		public const int DefaultTimeout = 5000;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of the application.
		/// </summary>
		/// <param name="process"> The process for the application. </param>
		public Application(Process process)
			: base(null, null)
		{
			Process = process;
			Application = this;

			if (Process != null)
			{
				//Process.Exited += (sender, args) => OnClosed();
				Process.EnableRaisingEvents = true;
			}

			Timeout = TimeSpan.FromMilliseconds(DefaultTimeout);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a flag to auto close the application when disposed of.
		/// </summary>
		public bool AutoClose { get; set; }

		/// <inheritdoc />
		public override Element FocusedElement => First(x => x.Focused);

		/// <summary>
		/// Gets the handle for this window.
		/// </summary>
		public IntPtr Handle => Process?.MainWindowHandle ?? IntPtr.Zero;

		/// <inheritdoc />
		public override string Id => Process?.Id.ToString();

		/// <summary>
		/// Gets the value indicating that the process is running.
		/// </summary>
		public bool IsRunning => (Process != null) && !Process.HasExited;

		/// <summary>
		/// Gets the location of the application.
		/// </summary>
		public Point Location => Process.GetWindowLocation();

		/// <inheritdoc />
		public override string Name => Handle.ToString();

		/// <summary>
		/// Gets the underlying process for this application.
		/// </summary>
		public Process Process { get; private set; }

		/// <summary>
		/// Gets the size of the application.
		/// </summary>
		public Size Size => Process.GetWindowSize();

		/// <summary>
		/// Gets or sets a flag to tell the browser to act slower. Defaults to false.
		/// </summary>
		public bool SlowMotion { get; set; }

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
		/// <param name="refresh"> The setting to determine to refresh children now. </param>
		/// <returns> The instance that represents the application. </returns>
		public static Application Attach(string executablePath, string arguments = null, bool refresh = true)
		{
			var fileName = Path.GetFileName(executablePath);
			if ((fileName != null) && !fileName.Contains("."))
			{
				fileName += ".exe";
			}

			var processName = Path.GetFileNameWithoutExtension(executablePath);
			var query = $"SELECT Handle, CommandLine FROM Win32_Process WHERE Name='{fileName}'";

			using (var searcher = new ManagementObjectSearcher(query))
			{
				foreach (var result in searcher.Get())
				{
					var managementObject = (ManagementObject) result;
					var handle = int.Parse(managementObject["Handle"].ToString());
					if (!string.IsNullOrWhiteSpace(arguments))
					{
						var data = managementObject["CommandLine"];
						if ((data == null) || !data.ToString().Contains(arguments))
						{
							continue;
						}
					}

					var process = Process.GetProcessesByName(processName).FirstOrDefault(x => x.Id == handle);
					if (process == null)
					{
						continue;
					}

					if ((process.MainWindowHandle == IntPtr.Zero) || !NativeMethods.IsWindowVisible(process.MainWindowHandle))
					{
						continue;
					}

					var application = new Application(process);
					if (!refresh)
					{
						return application;
					}

					application.Refresh();
					application.WaitForComplete();

					return application;
				}
			}

			return null;
		}

		/// <summary>
		/// Attaches the application to an existing process.
		/// </summary>
		/// <param name="handle"> The main window handle of the executable. </param>
		/// <param name="refresh"> The setting to determine to refresh children now. </param>
		/// <returns> The instance that represents the application. </returns>
		public static Application Attach(IntPtr handle, bool refresh = true)
		{
			var process = Process.GetProcesses().FirstOrDefault(x => x.MainWindowHandle == handle);
			return process == null ? null : Attach(process, refresh);
		}

		/// <summary>
		/// Attaches the application to an existing process.
		/// </summary>
		/// <param name="process"> The process to attach to. </param>
		/// <param name="refresh"> The setting to determine to refresh children now. </param>
		/// <returns> The instance that represents the application. </returns>
		public static Application Attach(Process process, bool refresh = true)
		{
			var application = new Application(process);
			if (!refresh)
			{
				return application;
			}

			application.Refresh();
			application.WaitForComplete();

			return application;
		}

		/// <summary>
		/// Attaches the application to an existing process.
		/// </summary>
		/// <param name="executablePath"> The path to the executable. </param>
		/// <param name="arguments"> The arguments for the executable. Arguments are optional. </param>
		/// <param name="refresh"> The setting to determine to refresh children now. </param>
		/// <returns> The instance that represents the application. </returns>
		public static Application AttachOrCreate(string executablePath, string arguments = null, bool refresh = true)
		{
			return Attach(executablePath, arguments, refresh) ?? Create(executablePath, arguments, refresh);
		}

		/// <summary>
		/// Brings the application to the front and makes it the top window.
		/// </summary>
		public Application BringToFront()
		{
			NativeMethods.SetFocus(Handle);
			NativeMethods.ShowWindow(Handle);
			NativeMethods.BringWindowToTop(Handle);
			NativeMethods.SetForegroundWindow(Handle);
			NativeMethods.BringToTop(Handle);
			return this;
		}

		/// <summary>
		/// Closes the window.
		/// </summary>
		public Application Close()
		{
			if (Process.HasExited)
			{
				return this;
			}

			Process.CloseMainWindow();
			if (!Process.WaitForExit(1500))
			{
				Process.Kill();
			}

			return this;
		}

		/// <summary>
		/// Closes all windows my name and closes them.
		/// </summary>
		/// <param name="executablePath"> The path to the executable. </param>
		/// <param name="timeout"> The timeout to wait for the application to close. </param>
		public static void CloseAll(string executablePath, int timeout = 1000)
		{
			var processName = Path.GetFileNameWithoutExtension(executablePath);

			// Find all the main processes.
			var processes = Process.GetProcessesByName(processName)
				.Where(x => x.MainWindowHandle != IntPtr.Zero);

			processes.ForEachDisposable(process =>
			{
				// Ask to close the process nicely.
				process.CloseMainWindow();

				if (!process.WaitForExit(timeout))
				{
					// The process did not close so now we are just going to kill it.
					process.Kill();
					process.WaitForExit();
				}
			});

			// Wait for the threads to sleep and child process to close.
			Thread.Sleep(250);

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
				application.WaitForComplete();
			}

			return application;
		}

		/// <summary>
		/// Checks to see if an application process exist by path and optional arguments.
		/// </summary>
		/// <param name="executablePath"> The path to the executable. </param>
		/// <param name="arguments"> The arguments for the executable. Arguments are optional. </param>
		/// <returns> True if the application exists and false otherwise. </returns>
		public static bool Exists(string executablePath, string arguments = null)
		{
			if (!executablePath.EndsWith(".exe"))
			{
				executablePath += ".exe";
			}

			var fileName = Path.GetFileName(executablePath);
			var processName = Path.GetFileNameWithoutExtension(executablePath);
			var query = $"SELECT Handle, CommandLine FROM Win32_Process WHERE Name='{fileName}'";

			using (var searcher = new ManagementObjectSearcher(query))
			{
				foreach (var result in searcher.Get())
				{
					var managementObject = (ManagementObject) result;
					var handle = int.Parse(managementObject["Handle"].ToString());
					if (!string.IsNullOrWhiteSpace(arguments))
					{
						var data = managementObject["CommandLine"];
						if ((data == null) || !data.ToString().Contains(arguments))
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

						if ((process.MainWindowHandle == IntPtr.Zero) || !NativeMethods.IsWindowVisible(process.MainWindowHandle))
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
		public Application MoveWindow(int x, int y, int width, int height)
		{
			NativeMethods.MoveWindow(Handle, x, y, width, height, true);
			return this;
		}

		/// <inheritdoc />
		public override ElementHost Refresh()
		{
			try
			{
				Utility.Wait(() =>
				{
					Children.Clear();
					Children.AddRange(Process.GetWindows(this));
					return Children.Any();
				}, Timeout.TotalMilliseconds, 10);

				WaitForComplete();
				Children.ForEach(x => x.Refresh());
				WaitForComplete();
				return this;
			}
			catch (COMException)
			{
				// A window close while trying to enumerate it. Wait for a second then try again.
				Thread.Sleep(250);
				return Refresh();
			}
		}

		/// <summary>
		/// Waits for the Process to not be busy.
		/// </summary>
		/// <param name="minimumDelay"> The minimum delay in milliseconds to wait. Defaults to 0 milliseconds. </param>
		public override ElementHost WaitForComplete(int minimumDelay = 0)
		{
			var watch = Stopwatch.StartNew();
			Process.WaitForInputIdle(Timeout.Milliseconds);

			while ((watch.Elapsed.TotalMilliseconds < minimumDelay) && (minimumDelay > 0))
			{
				Thread.Sleep(10);
			}

			return this;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing"> True if disposing and false if otherwise. </param>
		protected override void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}

			if (AutoClose && (Process != null) && Process.HasExited)
			{
				Close();
			}

			Process?.Dispose();
			Process = null;
		}
		
		#endregion
	}
}