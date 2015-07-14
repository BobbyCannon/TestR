#region References

using TestR.Desktop.Automation;
using TestR.Desktop.Automation.Patterns;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a check box element.
	/// </summary>
	public class CheckBox : Element
	{
		#region Fields

		private readonly TogglePattern _pattern;

		#endregion

		#region Constructors

		internal CheckBox(AutomationElement element, IElementParent parent)
			: base(element, parent)
		{
			var pattern = element.GetCurrentPattern(TogglePattern.Pattern);
			_pattern = (TogglePattern) pattern;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a value indicating if the check box is checked or not. True for checked and false otherwise.
		/// </summary>
		public bool Checked
		{
			get { return _pattern.Current.ToggleState != ToggleState.Off; }
		}

		/// <summary>
		/// Get the checked state of the check box.
		/// </summary>
		public ToggleState CheckedState
		{
			get { return _pattern.Current.ToggleState; }
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