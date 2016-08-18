#region References

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using TestR.Extensions;
using TestR.Helpers;
using TestR.Native;

#endregion

namespace TestR.Desktop
{
	/// <summary>
	/// Represents an application that can be automated.
	/// </summary>
	public class Application : IDisposable
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
		internal Application(Process process)
		{
			Children = new ElementCollection<Element>();
			Process = process;
			Process.Exited += (sender, args) => OnClosed();
			Process.EnableRaisingEvents = true;
			Timeout = TimeSpan.FromMilliseconds(DefaultTimeout);
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
		public ElementCollection<Element> Children { get; }

		/// <summary>
		/// Gets the handle for this window.
		/// </summary>
		public IntPtr Handle => Process.MainWindowHandle;

		/// <summary>
		/// Gets the ID of this application.
		/// </summary>
		public string Id => Process.Id.ToString();

		/// <summary>
		/// Gets the value indicating that the process is running.
		/// </summary>
		public bool IsRunning => Process != null && !Process.HasExited;

		/// <summary>
		/// Gets the location of the application.
		/// </summary>
		public Point Location => Process.GetWindowLocation();

		/// <summary>
		/// Gets the name of this element.
		/// </summary>
		public string Name => Handle.ToString();

		/// <summary>
		/// Gets the underlying process for this application.
		/// </summary>
		public Process Process { get; private set; }

		/// <summary>
		/// Gets the size of the application.
		/// </summary>
		public Size Size => Process.GetWindowSize();

		/// <summary>
		/// Gets or sets the time out for delay request. Defaults to 5 seconds.
		/// </summary>
		public TimeSpan Timeout { get; set; }

		#endregion

		#region Indexers

		/// <summary>
		/// Get a child using a provided key.
		/// </summary>
		/// <param name="id"> The ID of the child. </param>
		/// <returns> The child if found or null if otherwise. </returns>
		public Element this[string id] => Get(id, false);

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
			if (fileName != null && !fileName.Contains("."))
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
					if (!refresh)
					{
						return application;
					}

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
			application.WaitWhileBusy();

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
				application.WaitWhileBusy();
			}

			return application;
		}

		/// <summary>
		/// Gets a list of structure elements into a single collection.
		/// </summary>
		/// <returns> A collection of the items. </returns>
		public IEnumerable<Element> Descendants()
		{
			var nodes = new Stack<Element>(Children);
			while (nodes.Any())
			{
				var node = nodes.Pop();
				yield return node;

				foreach (var n in node.Children)
				{
					nodes.Push(n);
				}
			}
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
		/// Get the child from the children.
		/// </summary>
		/// <param name="id"> An ID of the element to get. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the ID. </returns>
		public Element Get(string id, bool recursive = true, bool wait = true)
		{
			return Get<Element>(id, recursive, wait);
		}

		/// <summary>
		/// Get the child from the children.
		/// </summary>
		/// <param name="condition"> A function to test each element for a condition. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the condition. </returns>
		public Element Get(Func<Element, bool> condition, bool recursive = true, bool wait = true)
		{
			return Get<Element>(condition, recursive, wait);
		}

		/// <summary>
		/// Get the child from the children.
		/// </summary>
		/// <param name="id"> An ID of the element to get. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the ID. </returns>
		public T Get<T>(string id, bool recursive = true, bool wait = true) where T : Element
		{
			return Get<T>(x => x.ApplicationId == id || x.Id == id || x.Name == id, recursive, wait);
		}

		/// <summary>
		/// Get the child from the children.
		/// </summary>
		/// <param name="condition"> A function to test each element for a condition. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the condition. </returns>
		public T Get<T>(Func<T, bool> condition, bool recursive = true, bool wait = true) where T : Element
		{
			T response = null;

			Utility.Wait(() =>
			{
				try
				{
					response = Children.Get(condition, recursive);
					if (response != null || !wait)
					{
						return true;
					}

					UpdateChildren();
					return false;
				}
				catch (Exception)
				{
					return !wait;
				}
			}, Timeout.TotalMilliseconds);

			return response;
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

		/// <summary>
		/// Refresh the list of items for the application.
		/// </summary>
		public Application Refresh()
		{
			try
			{
				Utility.Wait(() =>
				{
					Children.Clear();
					Children.AddRange(Process.GetWindows(this));
					return Children.Any();
				}, Timeout.TotalMilliseconds, 10);

				WaitWhileBusy();
				Children.ForEach(x => x.UpdateChildren());
				WaitWhileBusy();
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
		/// Update the children for this element.
		/// </summary>
		public Application UpdateChildren()
		{
			Refresh();
			OnChildrenUpdated();
			return this;
		}

		/// <summary>
		/// Waits for the Process to not be busy.
		/// </summary>
		/// <param name="minimumDelay"> The minimum delay in milliseconds to wait. Defaults to 0 milliseconds. </param>
		public Application WaitWhileBusy(int minimumDelay = 0)
		{
			var watch = Stopwatch.StartNew();
			Process.WaitForInputIdle(Timeout.Milliseconds);

			while (watch.Elapsed.TotalMilliseconds < minimumDelay && minimumDelay > 0)
			{
				Thread.Sleep(10);
			}

			return this;
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

			Process?.Dispose();
			Process = null;
		}

		/// <summary>
		/// Handles the children updated event.
		/// </summary>
		protected virtual void OnChildrenUpdated()
		{
			ChildrenUpdated?.Invoke();
		}

		/// <summary>
		/// Handles the closed event.
		/// </summary>
		protected virtual void OnClosed()
		{
			Closed?.Invoke();
		}

		/// <summary>
		/// Triggers th element clicked event.
		/// </summary>
		/// <param name="element"> The element that was clicked. </param>
		/// <param name="point"> The point that was clicked. </param>
		protected virtual void OnElementClicked(Element element, Point point)
		{
			if (element.ProcessId != Process.Id)
			{
				return;
			}

			ElementClicked?.Invoke(element, point);
		}

		private void MouseMonitorOnMouseChanged(Mouse.MouseEvent mouseEvent, Point point)
		{
			if (mouseEvent != Mouse.MouseEvent.LeftButtonDown && mouseEvent != Mouse.MouseEvent.RightButtonDown)
			{
				return;
			}

			var element = Element.FromPoint(point);
			if (element.ProcessId != Process.Id)
			{
				return;
			}

			OnElementClicked(element, point);
		}

		/// <summary>
		/// Handles the excited event.
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="e"> </param>
		private void OnExited(object sender, EventArgs e)
		{
			Exited?.Invoke();
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the children are updated.
		/// </summary>
		public event Action ChildrenUpdated;

		/// <summary>
		/// Event called when the application process closes.
		/// </summary>
		public event Action Closed;

		/// <summary>
		/// An element was clicked.
		/// </summary>
		public event Action<Element, Point> ElementClicked;

		/// <summary>
		/// Occurs when the application exits.
		/// </summary>
		public event Action Exited;

		#endregion
	}
}