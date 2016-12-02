#region References

using System.Drawing;
using System.Windows.Forms;

#endregion

namespace TestR.Native
{
	/// <summary>
	/// The filter to capture mouse messages.
	/// </summary>
	internal class MouseMessageFilter : IMessageFilter
	{
		#region Constants

		private const int LeftButtonDown = 0x201;
		private const int MouseMove = 0x200;
		private const int RightButtonDown = 0x204;

		#endregion

		#region Methods

		/// <summary>
		/// Filters out a message before it is dispatched.
		/// </summary>
		/// <returns>
		/// true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter
		/// or control.
		/// </returns>
		/// <param name="m"> The message to be dispatched. You cannot modify this message. </param>
		public bool PreFilterMessage(ref Message m)
		{
			Point mousePosition;

			switch (m.Msg)
			{
				case LeftButtonDown:
				case RightButtonDown:
					mousePosition = Control.MousePosition;
					var buttons = MouseButtons.None;

					switch (m.Msg)
					{
						case LeftButtonDown:
							buttons = MouseButtons.Left;
							break;
						case RightButtonDown:
							buttons = MouseButtons.Right;
							break;
					}

					Clicked?.Invoke(null, new MouseEventArgs(buttons, 0, mousePosition.X, mousePosition.Y, 0));
					break;

				case MouseMove:
					mousePosition = Control.MousePosition;
					Moved?.Invoke(null, new MouseEventArgs(MouseButtons.None, 0, mousePosition.X, mousePosition.Y, 0));
					break;
			}

			return false;
		}

		#endregion

		#region Events

		/// <summary>
		/// The mouse was clicked event.
		/// </summary>
		public event MouseEventHandler Clicked;

		/// <summary>
		/// The mouse was moved.
		/// </summary>
		public event MouseEventHandler Moved;

		#endregion
	}
}