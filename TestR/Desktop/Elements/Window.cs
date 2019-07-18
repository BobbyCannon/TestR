#region References

using System;
using System.Linq;
using System.Windows;
using TestR.Native;
using UIAutomationClient;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a window for an application.
	/// </summary>
	public class Window : DesktopElement
	{
		#region Constructors

		internal Window(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the handle for the Window.
		/// </summary>
		public IntPtr Handle => NativeElement.CurrentNativeWindowHandle;

		/// <summary>
		/// Get a flag that determines if the window is maximized.
		/// </summary>
		public bool IsMaximized => WindowState == WindowState.Maximized;

		/// <summary>
		/// Get a flag that determines if the window is minimized.
		/// </summary>
		public bool IsMinimized => WindowState == WindowState.Minimized;

		/// <summary>
		/// Gets the location of the element.
		/// </summary>
		public override Point Location
		{
			get
			{
				NativeMethods.Rect parentRect;
				var parentHandle = NativeMethods.GetParent(NativeElement.CurrentNativeWindowHandle);
				if (parentHandle != IntPtr.Zero)
				{
					NativeMethods.GetWindowRect(parentHandle, out parentRect);
				}
				else
				{
					parentRect = new NativeMethods.Rect();
				}

				NativeMethods.GetWindowRect(NativeElement.CurrentNativeWindowHandle, out NativeMethods.Rect rect);
				return new Point(rect.Left - parentRect.Left, rect.Top - parentRect.Top);
			}
		}

		/// <summary>
		/// Gets the status bar for the window. Returns null if the window does not have a status bar.
		/// </summary>
		public StatusBar StatusBar => FirstOrDefault<StatusBar>();

		/// <summary>
		/// Gets the title bar for the window. Returns null if the window does not have a title bar.
		/// </summary>
		public TitleBar TitleBar => FirstOrDefault<TitleBar>();

		/// <summary>
		/// Gets the state of the window.
		/// </summary>
		public WindowState WindowState
		{
			get
			{
				var state = NativeMethods.GetWindowPlacement(NativeElement.CurrentNativeWindowHandle).ShowState;

				switch (state)
				{
					case 1:
					default:
						return WindowState.Normal;

					case 2:
						return WindowState.Minimized;

					case 3:
						return WindowState.Maximized;
				}
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Bring the window to the front.
		/// </summary>
		public Window BringToFront()
		{
			var handle = NativeElement.CurrentNativeWindowHandle;
			NativeMethods.SetForegroundWindow(handle);
			NativeMethods.BringWindowToTop(handle);
			return this;
		}

		/// <summary>
		/// Closes a window.
		/// </summary>
		public Window Close()
		{
			if (TitleBar == null)
			{
				return this;
			}

			TitleBar.CloseButton.MoveMouseTo();
			BringToFront();
			TitleBar.CloseButton.Click();
			return this;
		}

		/// <summary>
		/// Move the window.
		/// </summary>
		/// <param name="x"> The x value of the position to move to. </param>
		/// <param name="y"> The y value of the position to move to. </param>
		public void Move(int x, int y)
		{
			NativeMethods.MoveWindow(NativeElement.CurrentNativeWindowHandle, x, y, Width, Height, true);
		}

		/// <summary>
		/// Move the window.
		/// </summary>
		/// <param name="x"> The x value of the position to move to. </param>
		/// <param name="y"> The y value of the position to move to. </param>
		/// <param name="width"> The width to set. </param>
		/// <param name="height"> The height to set. </param>
		public void Move(int x, int y, int width, int height)
		{
			NativeMethods.MoveWindow(NativeElement.CurrentNativeWindowHandle, x, y, width, height, true);
		}

		/// <summary>
		/// Move the window and resize it.
		/// </summary>
		/// <param name="location"> The location to move to. </param>
		/// <param name="size"> The size of the window. </param>
		public void Move(Point location, Size size)
		{
			NativeMethods.MoveWindow(NativeElement.CurrentNativeWindowHandle, location.X, location.Y, size.Width, size.Height, true);
		}

		/// <summary>
		/// Resize the window.
		/// </summary>
		/// <param name="width"> The width to set. </param>
		/// <param name="height"> The height to set. </param>
		public void Resize(int width, int height)
		{
			NativeMethods.MoveWindow(NativeElement.CurrentNativeWindowHandle, Location.X, Location.Y, width, height, true);
		}

		/// <summary>
		/// Waits for the window to no longer be busy.
		/// </summary>
		public Window WaitWhileBusy()
		{
			WaitForWindow();
			HourGlassWait();
			return this;
		}

		private void HourGlassWait()
		{
			Wait(x => MouseCursor.WaitCursors.Contains(Mouse.Cursor));
		}

		private void WaitForWindow()
		{
			// todo: why does this not work for window?
		}

		#endregion
	}
}