#region References

using TestR.Desktop.Pattern;
using UIAutomationClient;
using ToggleState = TestR.Desktop.Pattern.ToggleState;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a button element.
	/// </summary>
	public class Button : Element
	{
		#region Constructors

		internal Button(IUIAutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a flag indicating if the button is checked. Usable for split buttons.
		/// </summary>
		public bool Toggled => TogglePattern.New(this).Toggled;

		/// <summary>
		/// Gets the toggle state of the button.
		/// </summary>
		public ToggleState ToggleState => TogglePattern.New(this)?.ToggleState ?? ToggleState.Off;

		#endregion
	}
}