#region References

using TestR.Extensions;
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
		public bool Toggled => GetPattern<IUIAutomationTogglePattern>()?.CurrentToggleState.Convert() == ToggleState.Off;

		/// <summary>
		/// Gets the toggle state of the button.
		/// </summary>
		public ToggleState ToggleState => GetPattern<IUIAutomationTogglePattern>()?.CurrentToggleState.Convert() ?? ToggleState.Off;

		#endregion
	}
}