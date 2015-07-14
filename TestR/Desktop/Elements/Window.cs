#region References

using System;
using System.Linq;
using TestR.Desktop.Automation;
using TestR.Desktop.Automation.Patterns;
using TestR.Native;
using Utility = TestR.Helpers.Utility;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a window for an application.
	/// </summary>
	public class Window : Element
	{
		#region Fields

		private readonly WindowPattern _pattern;

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates a window element.
		/// </summary>
		/// <param name="element"> The automation element for this element. </param>
		/// <param name="parent"> The parent for this element. </param>
		internal Window(AutomationElement element, IElementParent parent)
			: base(element, parent)
		{
			var pattern = element.GetCurrentPattern(WindowPattern.Pattern);
			_pattern = (WindowPattern) pattern;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the interaction state of the browser.
		/// </summary>
		public WindowInteractionState InteractionState
		{
			get { return _pattern.Current.WindowInteractionState; }
		}

		/// <summary>
		/// Gets a value indicating the window is a modal.
		/// </summary>
		public bool IsModal
		{
			get { return _pattern.Current.IsModal; }
		}

		/// <summary>
		/// Gets the status bar for the window. Returns null if the window does not have a status bar.
		/// </summary>
		public StatusBar StatusBar
		{
			get { return Children.StatusBars.FirstOrDefault(); }
		}

		/// <summary>
		/// Gets the title bar for the window. Returns null if the window does not have a title bar.
		/// </summary>
		public TitleBar TitleBar
		{
			get { return Children.TitleBars.FirstOrDefault(); }
		}

		/// <summary>
		/// Gets or sets the visual state of the browser.
		/// </summary>
		public WindowVisualState VisualState
		{
			get { return _pattern.Current.WindowVisualState; }
			set { _pattern.SetWindowVisualState(value); }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Bring the window to the front.
		/// </summary>
		public void BringToFront()
		{
			var handle = new IntPtr(Automation.Current.NativeWindowHandle);
			NativeMethods.SetForegroundWindow(handle);
			NativeMethods.BringWindowToTop(handle);
		}

		/// <summary>
		/// Closes a window.
		/// </summary>
		public void Close()
		{
			if (TitleBar != null)
			{
				TitleBar.CloseButton.MoveMouseTo();
				//TitleBar.CloseButton.Click();
			}
			else
			{
				_pattern.Close();
			}
		}

		/// <summary>
		/// Creates and initialize a window element.
		/// </summary>
		/// <param name="element"> The automation element for the window. </param>
		/// <param name="parent"> The element this window belongs to. </param>
		/// <returns> The window for the automation element. </returns>
		public static Window Create(AutomationElement element, Element parent = null)
		{
			var window = new Window(element, parent);
			window.UpdateChildren();
			return window;
		}

		/// <summary>
		/// Waits for the window to no longer be busy.
		/// </summary>
		public void WaitWhileBusy()
		{
			WaitForWindow();
			HourGlassWait();
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

			Utility.Wait(() => _pattern.Current.WindowInteractionState != WindowInteractionState.NotResponding);
		}

		#endregion
	}
}