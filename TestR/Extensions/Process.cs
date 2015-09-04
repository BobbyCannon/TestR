#region References

using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using TestR.Native;
using UIAutomationClient;

#endregion

namespace TestR.Extensions
{
	public static partial class Helper
	{
		#region Methods

		/// <summary>
		/// Get the main window location for the process.
		/// </summary>
		/// <param name="process"> The process that contains the window. </param>
		/// <returns> The location of the window. </returns>
		public static Point GetWindowLocation(this Process process)
		{
			var p = NativeMethods.GetWindowPlacement(process.MainWindowHandle);
			var location = p.rcNormalPosition.Location;

			if (p.ShowState == 2 || p.ShowState == 3)
			{
				var windowsRect = new NativeMethods.Rect();
				NativeMethods.GetWindowRect(process.MainWindowHandle, out windowsRect);
				location = new Point(windowsRect.Left + 8, windowsRect.Top + 8);
			}

			return location;
		}

		/// <summary>
		/// Gets all windows for the process.
		/// </summary>
		/// <param name="process"> The process to get windows for. </param>
		/// <returns> The array of windows. </returns>
		public static IEnumerable<IUIAutomationElement> GetWindows(this Process process)
		{
			// There is a issue in Windows 10 and Cortana (or modern apps) where there is a 60 second delay when walking the root element. 
			// When you hit the last sibling it delays. For now we are simply going to return the main window and we'll roll this code 
			// back once the Windows 10 issue has been resolved.

			process.Refresh();
			var automation = new CUIAutomationClass();
			return new[] { automation.ElementFromHandle(process.MainWindowHandle) };
		}

		/// <summary>
		/// Gets the size of the main window for the process.
		/// </summary>
		/// <param name="process"> The process to size. </param>
		/// <returns> The size of the main window. </returns>
		public static Size GetWindowSize(this Process process)
		{
			var data = new NativeMethods.Rect();
			NativeMethods.GetWindowRect(process.MainWindowHandle, out data);
			return new Size(data.Right - data.Left, data.Bottom - data.Top);
		}

		#endregion
	}
}