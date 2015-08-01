#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the title bar for a window.
	/// </summary>
	public class TitleBar : Element
	{
		#region Constructors

		internal TitleBar(IUIAutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the close button.
		/// </summary>
		public Button CloseButton => Children.Buttons["Close"];

		/// <summary>
		/// Gets the maximize button.
		/// </summary>
		public Button MaximizeButton => Children.Buttons["Maximize"];

		/// <summary>
		/// Gets the maximize button.
		/// </summary>
		public Button MinimizeButton => Children.Buttons["Minimize"];

		/// <summary>
		/// Gets the text value.
		/// </summary>
		public string Text => Name;

		#endregion
	}
}