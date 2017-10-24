#region References

using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using FormApplication = System.Windows.Forms.Application;

#endregion

namespace TestR.Native
{
	/// <summary>
	/// Represents the mouse and allows for simulated input.
	/// </summary>
	public static class Mouse
	{
		#region Fields

		private static MouseMessageFilter _filter;
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
			return NativeMethods.GetCursorPosition(out var currentMousePoint) ? currentMousePoint : new Point();
		}

		/// <summary>
		/// Left click at the current mouse location.
		/// </summary>
		public static void LeftClick()
		{
			ExecuteMouseEvent(MouseEventFlags.LeftDown);
			ExecuteMouseEvent(MouseEventFlags.LeftUp);
		}

		/// <summary>
		/// Left click at the provided point.
		/// </summary>
		/// <param name="x"> The x point in which to click. </param>
		/// <param name="y"> The y point in which to click. </param>
		public static void LeftClick(int x, int y)
		{
			LeftClick(new Point(x, y));
		}

		/// <summary>
		/// Left click at the provided point.
		/// </summary>
		/// <param name="point"> The point in which to click. </param>
		public static void LeftClick(Point point)
		{
			MoveTo(point);
			ExecuteMouseEvent(MouseEventFlags.LeftDown, point);
			ExecuteMouseEvent(MouseEventFlags.LeftUp, point);
		}

		/// <summary>
		/// Left click and hold.
		/// </summary>
		public static void LeftClickDown()
		{
			ExecuteMouseEvent(MouseEventFlags.LeftDown);
		}

		/// <summary>
		/// Left click and hold.
		/// </summary>
		/// <param name="x"> The x point in which to click. </param>
		/// <param name="y"> The y point in which to click. </param>
		public static void LeftClickDown(int x, int y)
		{
			MoveTo(x, y);
			ExecuteMouseEvent(MouseEventFlags.LeftDown, new Point(x, y));
		}

		/// <summary>
		/// Left click release.
		/// </summary>
		public static void LeftClickUp()
		{
			ExecuteMouseEvent(MouseEventFlags.LeftUp);
		}

		/// <summary>
		/// Left click release.
		/// </summary>
		/// <param name="x"> The x point in which to click. </param>
		/// <param name="y"> The y point in which to click. </param>
		public static void LeftClickUp(int x, int y)
		{
			MoveTo(x, y);
			ExecuteMouseEvent(MouseEventFlags.LeftUp, new Point(x, y));
		}

		/// <summary>
		/// Middle click at the current mouse location.
		/// </summary>
		public static void MiddleClick()
		{
			ExecuteMouseEvent(MouseEventFlags.MiddleDown);
			ExecuteMouseEvent(MouseEventFlags.MiddleUp);
		}

		/// <summary>
		/// Middle click at the provided point.
		/// </summary>
		/// <param name="x"> The x point in which to click. </param>
		/// <param name="y"> The y point in which to click. </param>
		public static void MiddleClick(int x, int y)
		{
			MiddleClick(new Point(x, y));
		}

		/// <summary>
		/// Middle click at the provided point.
		/// </summary>
		/// <param name="point"> The point in which to click. </param>
		public static void MiddleClick(Point point)
		{
			MoveTo(point);
			ExecuteMouseEvent(MouseEventFlags.MiddleDown, point);
			ExecuteMouseEvent(MouseEventFlags.MiddleUp, point);
		}

		/// <summary>
		/// Middle click and hold.
		/// </summary>
		public static void MiddleClickDown()
		{
			ExecuteMouseEvent(MouseEventFlags.MiddleDown);
		}

		/// <summary>
		/// Middle click release.
		/// </summary>
		public static void MiddleClickUp()
		{
			ExecuteMouseEvent(MouseEventFlags.MiddleUp);
		}

		/// <summary>
		/// Sets the mouse to the provide point.
		/// </summary>
		/// <param name="x"> The x point in which to move to. </param>
		/// <param name="y"> The y point in which to move to. </param>
		public static void MoveTo(int x, int y)
		{
			MoveTo(new Point(x, y));
		}

		/// <summary>
		/// Sets the mouse to the provide point.
		/// </summary>
		/// <param name="point"> The point in which to move to. </param>
		public static void MoveTo(Point point)
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

			//ExecuteMouseEvent(MouseEventFlags.Move, point);
		}

		/// <summary>
		/// Right click at the current mouse location.
		/// </summary>
		public static void RightClick()
		{
			ExecuteMouseEvent(MouseEventFlags.RightDown);
			ExecuteMouseEvent(MouseEventFlags.RightUp);
		}

		/// <summary>
		/// Right click at the provided point.
		/// </summary>
		/// <param name="x"> The x point in which to click. </param>
		/// <param name="y"> The y point in which to click. </param>
		public static void RightClick(int x, int y)
		{
			RightClick(new Point(x, y));
		}

		/// <summary>
		/// Right click at the provided point.
		/// </summary>
		/// <param name="point"> The point in which to click. </param>
		public static void RightClick(Point point)
		{
			MoveTo(point);
			ExecuteMouseEvent(MouseEventFlags.RightDown, point);
			ExecuteMouseEvent(MouseEventFlags.RightUp, point);
		}

		/// <summary>
		/// Right click and hold.
		/// </summary>
		public static void RightClickDown()
		{
			ExecuteMouseEvent(MouseEventFlags.RightDown);
		}

		/// <summary>
		/// Right click release.
		/// </summary>
		public static void RightClickUp()
		{
			ExecuteMouseEvent(MouseEventFlags.RightUp);
		}

		/// <summary>
		/// Select a section of screen using the left mouse button.
		/// </summary>
		/// <param name="x1"> Start x location. </param>
		/// <param name="y1"> Start y location. </param>
		/// <param name="x2"> End x location. </param>
		/// <param name="y2"> End y location. </param>
		public static void Select(int x1, int y1, int x2, int y2)
		{
			LeftClickDown(x1, y1);
			Thread.Sleep(50);
			LeftClickUp(x2, y2);
		}

		/// <summary>
		/// Start monitoring the mouse for events.
		/// </summary>
		public static void StartMonitoring()
		{
			if (_filter != null)
			{
				return;
			}

			_filter = new MouseMessageFilter();
			_filter.Clicked += (sender, args) => Clicked?.Invoke(sender, args);
			_filter.Moved += (sender, args) => Moved?.Invoke(sender, args);

			FormApplication.AddMessageFilter(_filter);
		}

		/// <summary>
		/// Stop monitoring the mouse for events.
		/// </summary>
		public static void StopMonitoring()
		{
			if (_filter == null)
			{
				return;
			}

			FormApplication.RemoveMessageFilter(_filter);
		}

		private static void ExecuteMouseEvent(MouseEventFlags value, Point? point = null)
		{
			var position = point ?? GetCursorPosition();
			NativeMethods.MouseEvent((int) value, position.X, position.Y, 0, 0);
		}

		#endregion

		#region Events

		/// <summary>
		/// Event for key press events when monitoring the keyboard.
		/// </summary>
		public static event MouseEventHandler Clicked;

		/// <summary>
		/// The mouse was moved.
		/// </summary>
		public static event MouseEventHandler Moved;

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
			RightDown = 0x00000008,
			RightUp = 0x00000010
		}

		#endregion
	}
}