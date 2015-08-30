#region References

using System;
using System.Diagnostics;
using System.Drawing;

#endregion

namespace TestR.Native
{
	/// <summary>
	/// Represents the mouse and allows for simulated input.
	/// </summary>
	public static class Mouse
	{
		#region Fields

		private static readonly TimeSpan _timeout;

		#endregion

		#region Constructors

		static Mouse()
		{
			_timeout = new TimeSpan(0, 0, 5);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the current cursor for the mouse.
		/// </summary>
		public static MouseCursor Cursor => MouseCursor.Current;

		#endregion

		#region Methods

		/// <summary>
		/// Gets the current position of the mouse.
		/// </summary>
		/// <returns> The point location of the mouse cursor. </returns>
		public static Point GetCursorPosition()
		{
			Point currentMousePoint;
			return NativeMethods.GetCursorPosition(out currentMousePoint) ? currentMousePoint : new Point();
		}

		/// <summary>
		/// Left click at the provided point.
		/// </summary>
		/// <param name="point"> The point in which to click. </param>
		public static void LeftClick(Point point)
		{
			var watch = Stopwatch.StartNew();
			var currentPosition = GetCursorPosition();

			while (currentPosition.X != point.X || currentPosition.Y != point.Y)
			{
				NativeMethods.SetCursorPosition(point.X, point.Y);
				currentPosition = GetCursorPosition();

				if (watch.Elapsed >= _timeout)
				{
					throw new TimeoutException("Failed to move the mouse to the correct location.");
				}
			}

			ExecuteMouseEvent(MouseEventFlags.LeftDown);
			ExecuteMouseEvent(MouseEventFlags.LeftUp);
		}

		/// <summary>
		/// Sets the mouse to the provide point.
		/// </summary>
		/// <param name="point"> The point in which to move to. </param>
		public static void MoveTo(Point point)
		{
			NativeMethods.SetCursorPosition(point.X, point.Y);
		}

		/// <summary>
		/// Right click at the provided point.
		/// </summary>
		/// <param name="point"> The point in which to click. </param>
		public static void RightClick(Point point)
		{
			NativeMethods.SetCursorPosition(point.X, point.Y);
			ExecuteMouseEvent(MouseEventFlags.RightDown);
			ExecuteMouseEvent(MouseEventFlags.RightUp);
		}

		internal static MouseEvent GetEvent(NativeMethods.MouseMessages message)
		{
			switch (message)
			{
				case NativeMethods.MouseMessages.WM_LBUTTONDOWN:
					return MouseEvent.LeftButtonDown;

				case NativeMethods.MouseMessages.WM_LBUTTONUP:
					return MouseEvent.LeftButtonUp;

				case NativeMethods.MouseMessages.WM_MOUSEMOVE:
					return MouseEvent.MouseMove;

				case NativeMethods.MouseMessages.WM_MOUSEWHEEL:
					return MouseEvent.MouseWheel;

				case NativeMethods.MouseMessages.WM_RBUTTONDOWN:
					return MouseEvent.RightButtonDown;

				case NativeMethods.MouseMessages.WM_RBUTTONUP:
					return MouseEvent.RightButtonUp;

				default:
					return MouseEvent.Unknown;
			}
		}

		private static void ExecuteMouseEvent(MouseEventFlags value)
		{
			var position = GetCursorPosition();
			NativeMethods.MouseEvent((int) value, position.X, position.Y, 0, 0);
		}

		#endregion

		#region Enumerations

		/// <summary>
		/// Represents a mouse event.
		/// </summary>
		public enum MouseEvent
		{
			/// <summary>
			/// Unknown event.
			/// </summary>
			Unknown,

			/// <summary>
			/// Event for left button press.
			/// </summary>
			LeftButtonDown,

			/// <summary>
			/// Event for left button release.
			/// </summary>
			LeftButtonUp,

			/// <summary>
			/// Event for mouse moving.
			/// </summary>
			MouseMove,

			/// <summary>
			/// Event for mouse wheel moving.
			/// </summary>
			MouseWheel,

			/// <summary>
			/// Event for right button press.
			/// </summary>
			RightButtonDown,

			/// <summary>
			/// Event for right button release.
			/// </summary>
			RightButtonUp
		}

		[Flags]
		private enum MouseEventFlags
		{
			LeftDown = 0x00000002,
			LeftUp = 0x00000004,
			MiddleDown = 0x00000020,
			MiddleUp = 0x00000040,
			Move = 0x00000001,
			Absolute = 0x00008000,
			RightDown = 0x00000008,
			RightUp = 0x00000010
		}

		#endregion
	}
}