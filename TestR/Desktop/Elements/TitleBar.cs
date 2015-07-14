#region References

using TestR.Desktop.Automation;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the title bar for a window.
	/// </summary>
	public class TitleBar : Element
	{
		#region Constructors

		internal TitleBar(AutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the close button.
		/// </summary>
		public Button CloseButton
		{
			get { return Children.Buttons["Close"]; }
		}

		/// <summary>
		/// Gets the maximize button.
		/// </summary>
		public Button MaximizeButton
		{
			get { return Children.Buttons["Maximize"]; }
		}

		/// <summary>
		/// Gets the maximize button.
		/// </summary>
		public Button MinimizeButton
		{
			get { return Children.Buttons["Minimize"]; }
		}

		/// <summary>
		/// Gets the text value.
		/// </summary>
		public string Text
		{
			get { return Name; }
		}

		#endregion
	}
}