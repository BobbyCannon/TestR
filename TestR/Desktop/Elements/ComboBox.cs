#region References

using TestR.Desktop.Pattern;
using UIAutomationClient;
using ExpandCollapseState = TestR.Desktop.Pattern.ExpandCollapseState;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a combo box element.
	/// </summary>
	public class ComboBox : Element
	{
		#region Constructors

		internal ComboBox(IUIAutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the current expanded state of the combo box.
		/// </summary>
		public ExpandCollapseState ExpandCollapseState => ExpandCollapsePattern.New(this)?.ExpandCollapseState ?? ExpandCollapseState.Collapsed;

		/// <summary>
		/// Gets a value indicating whether the control can have a value set programmatically, or that can be edited by the user.
		/// </summary>
		public bool ReadOnly => ValuePattern.New(this)?.IsReadOnly ?? false;

		/// <summary>
		/// Gets the text value.
		/// </summary>
		public string Text => ValuePattern.New(this)?.Value ?? string.Empty;

		#endregion

		#region Methods

		/// <summary>
		/// Collapses the combo box.
		/// </summary>
		public void Collapse()
		{
			ExpandCollapsePattern.New(this)?.Collapse();
		}

		/// <summary>
		/// Expands the combo box.
		/// </summary>
		public void Expand()
		{
			ExpandCollapsePattern.New(this)?.Expand();
		}

		#endregion
	}
}