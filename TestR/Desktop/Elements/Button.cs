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
	public class Button : DesktopElement
	{
		#region Constructors

		internal Button(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a flag indicating if the button is checked. Usable for split buttons.
		/// </summary>
		public bool Toggled => TogglePattern.Create(this)?.Toggled ?? false;

		/// <summary>
		/// Gets the toggle state of the button.
		/// </summary>
		public ToggleState ToggleState => TogglePattern.Create(this)?.ToggleState ?? ToggleState.Off;

		#endregion
	}
}