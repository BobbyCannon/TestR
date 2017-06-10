#region References

using System;
using System.Diagnostics;
using System.Drawing;
using static TestR.Native.NativeMethods;
using Point = System.Drawing.Point;

#endregion

namespace TestR.Native
{
	/// <summary>
	/// Provided process details to safely work in x86 or x64 processes.
	/// </summary>
	public class SafeProcess : IDisposable
	{
		#region Fields

		private Process _process;

		#endregion

		#region Constructors

		internal SafeProcess(int id)
		{
			Id = id;
			_process = null;
		}

		internal SafeProcess(Process process)
		{
			Id = process.Id;
			_process = process;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the arguments that were provided when the process started.
		/// </summary>
		public string Arguments { get; set; }

		/// <summary>
		/// Gets the name of the file.
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Gets the path to the file.
		/// </summary>
		public string FilePath { get; set; }

		/// <summary>
		/// Gets the handle to this proces interaction.
		/// </summary>
		public IntPtr Handle => Process.Handle;

		/// <summary>
		/// Gets a value indicating whether the associated process has been terminated.
		/// </summary>
		public bool HasExited => Process.HasExited;

		/// <summary>
		/// Gets the ID.
		/// </summary>
		public int Id { get; }

		/// <summary>
		/// Gets a flag idicating the process is elevated.
		/// </summary>
		public bool IsElevated => IsElevated(Handle);

		/// <summary>
		/// Gets the main window handle.
		/// </summary>
		public IntPtr MainWindowHandle => Process.MainWindowHandle;

		/// <summary>
		/// Gets the name
		/// </summary>
		public string Name { get; internal set; }

		/// <summary>
		/// Gets the details of the safe process.
		/// </summary>
		/// <seealso cref="System.Diagnostics.Process" />
		public Process Process => _process ?? (_process = Process.GetProcessById(Id));

		#endregion

		#region Methods

		/// <summary>
		/// Wait for the process to close if not then kill the process.
		/// </summary>
		/// <param name="timeout"> The timout to wait for graceful close. If the timeout is reached then kill the process. The timeout is in milliseconds. </param>
		public void Close(int timeout = 0)
		{
			try
			{
				// See if the process has exited
				if (_process == null || _process.HasExited)
				{
					return;
				}

				// Ask the process to close gracefully and give it 10 seconds to do so.
				_process.Refresh();
				_process.CloseMainWindow();
				_process.WaitForExit(timeout);
			}
			catch
			{
				// Ignore any errors
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
		/// Gets the process for this safe process.
		/// </summary>
		/// <returns> </returns>
		public Process GetProcess()
		{
			return Process;
		}

		/// <summary>
		/// First the main window location for the process.
		/// </summary>
		/// <returns> The location of the window. </returns>
		public Point GetWindowLocation()
		{
			var p = GetWindowPlacement(Process.MainWindowHandle);
			var location = p.rcNormalPosition.Location;

			if (p.ShowState == 2 || p.ShowState == 3)
			{
				GetWindowRect(Process.MainWindowHandle, out Rect windowsRect);
				location = new Point(windowsRect.Left + 8, windowsRect.Top + 8);
			}

			return location;
		}

		/// <summary>
		/// Gets the size of the main window for the process.
		/// </summary>
		/// <returns> The size of the main window. </returns>
		public Size GetWindowSize()
		{
			GetWindowRect(MainWindowHandle, out Rect data);
			return new Size(data.Right - data.Left, data.Bottom - data.Top);
		}

		/// <summary>
		/// Wait for the process to close if not then kill the process.
		/// </summary>
		/// <param name="timeout"> The timout to wait for graceful close. If the timeout is reached then kill the process. The timeout is in milliseconds. </param>
		public void Kill(int timeout = 0)
		{
			try
			{
				// See if the process has already shutdown.
				if (_process == null || _process.HasExited)
				{
					return;
				}

				// OK, no more Mr. Nice Guy time to just kill the process.
				_process.Kill();
				_process.WaitForExit(timeout);
			}
			catch
			{
				// Ignore any errors
			}
		}

		/// <summary>
		/// Wait for the process to exit.
		/// </summary>
		/// <param name="timeout"> The timout to wait for exit. The timeout is in milliseconds. </param>
		public void WaitForExit(int timeout = 10000)
		{
			try
			{
				// See if the process has exited
				if (_process == null || _process.HasExited)
				{
					return;
				}

				_process.WaitForExit(timeout);
			}
			catch
			{
				// Ignore any errors
			}
		}

		/// <summary>
		/// Wait for the process to go idle.
		/// </summary>
		/// <param name="milliseconds"> The time to wait for idle to occur. Time is in milliseconds. </param>
		public void WaitForInputIdle(int milliseconds)
		{
			Process?.WaitForInputIdle(milliseconds);
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

			_process?.Dispose();
			_process = null;
		}

		#endregion
	}
}