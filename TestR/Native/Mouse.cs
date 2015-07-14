#region References

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

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
		public static MouseCursor Cursor
		{
			get { return MouseCursor.Current; }
		}

		#endregion

		#region Methods

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
				SetCursorPosition(point.X, point.Y);
				currentPosition = GetCursorPosition();

				if (watch.Elapsed >= _timeout)
				{
					throw new TimeoutException("Failed to move the mouse to the correct location.");
				}
			}

			MouseEvent(MouseEventFlags.LeftDown);
			MouseEvent(MouseEventFlags.LeftUp);
		}

		/// <summary>
		/// Right click at the provided point.
		/// </summary>
		/// <param name="point"> The point in which to click. </param>
		public static void RightClick(Point point)
		{
			SetCursorPosition(point.X, point.Y);
			MouseEvent(MouseEventFlags.RightDown);
			MouseEvent(MouseEventFlags.RightUp);
		}

		/// <summary>
		/// Sets the mouse to the provide point.
		/// </summary>
		/// <param name="point"> The point in which to move to. </param>
		public static void MoveTo(Point point)
		{
			SetCursorPosition(point.X, point.Y);
		}

		private static Point GetCursorPosition()
		{
			Point currentMousePoint;
			return GetCursorPosition(out currentMousePoint)
				? currentMousePoint
				: new Point();
		}

		[DllImport("user32.dll", EntryPoint = "GetCursorPos")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetCursorPosition(out Point lpMousePoint);

		private static void MouseEvent(MouseEventFlags value)
		{
			var position = GetCursorPosition();
			MouseEvent((int) value, position.X, position.Y, 0, 0);
		}

		[DllImport("user32.dll", EntryPoint = "mouse_event")]
		private static extern void MouseEvent(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

		[DllImport("user32.dll", EntryPoint = "SetCursorPos")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetCursorPosition(int x, int y);

		#endregion

		#region Enumerations

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