#region References

using TestR.Extensions;
using UIAutomationClient;
using ToggleState = TestR.Desktop.Pattern.ToggleState;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a check box element.
	/// </summary>
	public class CheckBox : Element
	{
		#region Constructors

		internal CheckBox(IUIAutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a flag indicating if the checkbox is checked.
		/// </summary>
		public bool Checked => GetPattern<IUIAutomationTogglePattern>()?.CurrentToggleState.Convert() != ToggleState.Off;

		/// <summary>
		/// Gets the state of the checkbox.
		/// </summary>
		public ToggleState CheckedState => GetPattern<IUIAutomationTogglePattern>()?.CurrentToggleState.Convert() ?? ToggleState.Off;

		/// <summary>
		/// Gets a value indicating whether the control can have a value set programmatically, or that can be edited by the user.
		/// </summary>
		public bool ReadOnly => GetPattern<IUIAutomationValuePattern>()?.CurrentIsReadOnly == 1;

		/// <summary>
		/// Gets the text value.
		/// </summary>
		public string Text => GetPattern<IUIAutomationValuePattern>()?.CurrentValue ?? Name;

		#endregion

		#region Methods

		/// <summary>
		/// Toggle the checkbox.
		/// </summary>
		public void Toggle()
		{
			GetPattern<IUIAutomationTogglePattern>()?.Toggle();
		}

		#endregion
	}
}