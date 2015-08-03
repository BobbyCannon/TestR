#region References

using TestR.Desktop.Pattern;
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
		public bool Checked => TogglePattern.New(this).ToggleState != ToggleState.Off;

		/// <summary>
		/// Gets the state of the checkbox.
		/// </summary>
		public ToggleState CheckedState => TogglePattern.New(this).ToggleState;

		/// <summary>
		/// Gets a value indicating whether the control can have a value set programmatically, or that can be edited by the user.
		/// </summary>
		public bool ReadOnly => ValuePattern.New(this)?.IsReadOnly ?? true;

		/// <summary>
		/// Gets the text value.
		/// </summary>
		public string Text => ValuePattern.New(this)?.Value;

		#endregion

		#region Methods

		/// <summary>
		/// Toggle the checkbox.
		/// </summary>
		public void Toggle()
		{
			TogglePattern.New(this)?.Toggle();
		}

		#endregion
	}
}