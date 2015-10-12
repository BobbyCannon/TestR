#region References

using System.Linq;
using TestR.Desktop.Pattern;
using TestR.Helpers;
using TestR.Native;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a window for an application.
	/// </summary>
	public class Window : Element
	{
		#region Constructors

		internal Window(IUIAutomationElement element, Application application, Element parent)
			: base(element, application, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the status bar for the window. Returns null if the window does not have a status bar.
		/// </summary>
		public StatusBar StatusBar => Children.StatusBars.FirstOrDefault();

		/// <summary>
		/// Gets the title bar for the window. Returns null if the window does not have a title bar.
		/// </summary>
		public TitleBar TitleBar => Children.TitleBars.FirstOrDefault();

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
			TransformPattern.Create(this).Move(x, y);
		}

		/// <summary>
		/// Resize the window.
		/// </summary>
		/// <param name="width"> The width to set. </param>
		/// <param name="height"> The height to set. </param>
		public void Resize(int width, int height)
		{
			TransformPattern.Create(this).Resize(width, height);
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

		private static void HourGlassWait()
		{
			Utility.Wait(() => MouseCursor.WaitCursors.Contains(Mouse.Cursor));
		}

		private void WaitForWindow()
		{
			// todo: why does this not work for window?
			//if (_pattern.WaitForInputIdle(100) < 0)
			//{
			//	throw new Exception("Timed out waiting for window to respond.");
			//}

			//Utility.Wait(() => _pattern.Current.WindowInteractionState != WindowInteractionState.NotResponding);
		}

		#endregion
	}
}