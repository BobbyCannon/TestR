﻿#region References

using System;
using System.Drawing;
using System.Threading;
using TestR.Internal.Inputs;
using TestR.Internal.Native;

#endregion

namespace TestR.Desktop
{
	/// <summary>
	/// Represents the mouse and allows for simulated input.
	/// </summary>
	public class Mouse
	{
		#region Constants

		private const int MouseWheelClickSize = 120;

		#endregion

		#region Fields

		/// <summary>
		/// The instance of the <see cref="InputMessageDispatcher" /> to use for dispatching <see cref="Internal.Inputs.Input" /> messages.
		/// </summary>
		private readonly InputMessageDispatcher _messageDispatcher;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Mouse" /> class.
		/// </summary>
		public Mouse()
		{
			_messageDispatcher = new InputMessageDispatcher();
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
		public Point GetCursorPosition()
		{
			return NativeInput.GetCursorPosition(out var currentMousePoint) ? currentMousePoint : new Point();
		}

		/// <summary>
		/// Simulates a mouse horizontal wheel scroll gesture. Supported by Windows Vista and later.
		/// </summary>
		/// <param name="scrollAmountInClicks"> The amount to scroll in clicks. A positive value indicates that the wheel was rotated to the right; a negative value indicates that the wheel was rotated to the left. </param>
		public Mouse HorizontalScroll(int scrollAmountInClicks)
		{
			var inputList = new InputBuilder()
				.AddMouseHorizontalWheelScroll(scrollAmountInClicks * MouseWheelClickSize)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a mouse left-click gesture.
		/// </summary>
		public Mouse LeftButtonClick()
		{
			var inputList = new InputBuilder()
				.AddMouseButtonClick(MouseButton.LeftButton)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a mouse left-click gesture.
		/// </summary>
		/// <param name="point"> The absolute X-coordinate and Y-coordinate for the click. </param>
		public Mouse LeftButtonClick(Point point)
		{
			LeftButtonClick(point.X, point.Y);
			return this;
		}

		/// <summary>
		/// Simulates a mouse left-click gesture.
		/// </summary>
		/// <param name="x"> The absolute X-coordinate for the click. </param>
		/// <param name="y"> The absolute Y-coordinate for the click. </param>
		public Mouse LeftButtonClick(int x, int y)
		{
			MoveTo(x, y);

			var inputList = new InputBuilder()
				.AddMouseButtonClick(MouseButton.LeftButton)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a mouse left button double-click gesture.
		/// </summary>
		public Mouse LeftButtonDoubleClick()
		{
			var inputList = new InputBuilder()
				.AddMouseButtonDoubleClick(MouseButton.LeftButton)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a mouse left button down gesture.
		/// </summary>
		public Mouse LeftButtonDown()
		{
			var inputList = new InputBuilder()
				.AddMouseButtonDown(MouseButton.LeftButton)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a mouse left button up gesture.
		/// </summary>
		public Mouse LeftButtonUp()
		{
			var inputList = new InputBuilder()
				.AddMouseButtonUp(MouseButton.LeftButton)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a mouse left-click gesture.
		/// </summary>
		public Mouse MiddleButtonClick()
		{
			var inputList = new InputBuilder()
				.AddMouseButtonClick(MouseButton.MiddleButton)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a mouse middle-click gesture.
		/// </summary>
		/// <param name="point"> The absolute X-coordinate and Y-coordinate for the click. </param>
		public Mouse MiddleButtonClick(Point point)
		{
			MiddleButtonClick(point.X, point.Y);
			return this;
		}

		/// <summary>
		/// Simulates a mouse middle-click gesture.
		/// </summary>
		/// <param name="x"> The absolute X-coordinate for the click. </param>
		/// <param name="y"> The absolute Y-coordinate for the click. </param>
		public Mouse MiddleButtonClick(int x, int y)
		{
			MoveTo(x, y);

			var inputList = new InputBuilder()
				.AddMouseButtonClick(MouseButton.MiddleButton)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates mouse movement by the specified distance measured as a delta from the current mouse location in pixels.
		/// </summary>
		/// <param name="pixelDeltaX"> The distance in pixels to move the mouse horizontally. </param>
		/// <param name="pixelDeltaY"> The distance in pixels to move the mouse vertically. </param>
		public Mouse MoveBy(int pixelDeltaX, int pixelDeltaY)
		{
			var location = GetCursorPosition();
			MoveTo(location.X + pixelDeltaX, location.Y + pixelDeltaY);
			return this;
		}

		/// <summary>
		/// Simulates mouse movement to the specified location on the primary display device.
		/// </summary>
		/// <param name="point"> The absolute X-coordinate and Y-coordinate to move the mouse to. </param>
		public Mouse MoveTo(Point point)
		{
			return MoveTo(point.X, point.Y);
		}

		/// <summary>
		/// Simulates mouse movement to the specified location on the primary display device.
		/// </summary>
		/// <param name="point"> The absolute X-coordinate and Y-coordinate to move the mouse to. </param>
		public Mouse MoveTo(PointF point)
		{
			return MoveTo((int) point.X, (int) point.Y);
		}

		/// <summary>
		/// Simulates mouse movement to the specified location.
		/// </summary>
		/// <param name="x"> The absolute X-coordinate to move the mouse cursor to. </param>
		/// <param name="y"> The absolute Y-coordinate to move the mouse cursor to. </param>
		public Mouse MoveTo(int x, int y)
		{
			NativeInput.SetCursorPosition(x, y);
			return this;
		}

		/// <summary>
		/// Simulates a mouse right button click gesture.
		/// </summary>
		public Mouse RightButtonClick()
		{
			var inputList = new InputBuilder()
				.AddMouseButtonClick(MouseButton.RightButton)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a mouse move to an absolute position then right button click gesture.
		/// </summary>
		/// <param name="point"> The absolute X-coordinate and Y-coordinate for the click. </param>
		public Mouse RightButtonClick(Point point)
		{
			RightButtonClick(point.X, point.Y);
			return this;
		}

		/// <summary>
		/// Simulates a mouse move to an absolute position then right button click gesture.
		/// </summary>
		/// <param name="x"> The absolute X-coordinate for the click. </param>
		/// <param name="y"> The absolute Y-coordinate for the click. </param>
		public Mouse RightButtonClick(int x, int y)
		{
			MoveTo(x, y);

			var inputList = new InputBuilder()
				.AddMouseButtonClick(MouseButton.RightButton)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a mouse right button double-click gesture.
		/// </summary>
		public Mouse RightButtonDoubleClick()
		{
			var inputList = new InputBuilder()
				.AddMouseButtonDoubleClick(MouseButton.RightButton)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a mouse right button down gesture.
		/// </summary>
		public Mouse RightButtonDown()
		{
			var inputList = new InputBuilder()
				.AddMouseButtonDown(MouseButton.RightButton)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a mouse right button up gesture.
		/// </summary>
		public Mouse RightButtonUp()
		{
			var inputList = new InputBuilder()
				.AddMouseButtonUp(MouseButton.RightButton)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Sleeps the executing thread to create a pause between simulated inputs.
		/// </summary>
		/// <param name="timeoutInMilliseconds"> The number of milliseconds to wait. </param>
		public Mouse Sleep(int timeoutInMilliseconds)
		{
			Thread.Sleep(timeoutInMilliseconds);
			return this;
		}

		/// <summary>
		/// Sleeps the executing thread to create a pause between simulated inputs.
		/// </summary>
		/// <param name="timeout"> The time to wait. </param>
		public Mouse Sleep(TimeSpan timeout)
		{
			Thread.Sleep(timeout);
			return this;
		}

		/// <summary>
		/// Simulates mouse vertical wheel scroll gesture.
		/// </summary>
		/// <param name="scrollAmountInClicks">
		/// The amount to scroll in clicks. A positive value indicates that the wheel was rotated forward, away from the user; a negative
		/// value indicates that the wheel was rotated backward, toward the user.
		/// </param>
		public Mouse VerticalScroll(int scrollAmountInClicks)
		{
			var inputList = new InputBuilder()
				.AddMouseVerticalWheelScroll(scrollAmountInClicks * MouseWheelClickSize)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a mouse X button click gesture.
		/// </summary>
		/// <param name="buttonId"> The button id. </param>
		public Mouse XButtonClick(int buttonId)
		{
			var inputList = new InputBuilder()
				.AddMouseXButtonClick(buttonId)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a mouse X button double-click gesture.
		/// </summary>
		/// <param name="buttonId"> The button id. </param>
		public Mouse XButtonDoubleClick(int buttonId)
		{
			var inputList = new InputBuilder()
				.AddMouseXButtonDoubleClick(buttonId)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a mouse X button down gesture.
		/// </summary>
		/// <param name="buttonId"> The button id. </param>
		public Mouse XButtonDown(int buttonId)
		{
			var inputList = new InputBuilder()
				.AddMouseXButtonDown(buttonId)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a mouse X button up gesture.
		/// </summary>
		/// <param name="buttonId"> The button id. </param>
		public Mouse XButtonUp(int buttonId)
		{
			var inputList = new InputBuilder()
				.AddMouseXButtonUp(buttonId)
				.ToArray();

			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Sends the list of <see cref="Internal.Inputs.Input" /> messages using the <see cref="InputMessageDispatcher" /> instance.
		/// </summary>
		/// <param name="inputList"> The <see cref="System.Array" /> of <see cref="Internal.Inputs.Input" /> messages to send. </param>
		private void SendSimulatedInput(Internal.Inputs.Input[] inputList)
		{
			_messageDispatcher.DispatchInput(inputList);
		}

		#endregion
	}
}