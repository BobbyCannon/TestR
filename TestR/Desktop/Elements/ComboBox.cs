#region References

using TestR.Extensions;
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
		public ExpandCollapseState ExpandCollapseState => GetPattern<IUIAutomationExpandCollapsePattern>()?.CurrentExpandCollapseState.Convert() ?? ExpandCollapseState.Expanded;

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
		/// Collapses the combo box.
		/// </summary>
		public void Collapse()
		{
			GetPattern<IUIAutomationExpandCollapsePattern>()?.Collapse();
		}

		/// <summary>
		/// Expands the combo box.
		/// </summary>
		public void Expand()
		{
			GetPattern<IUIAutomationExpandCollapsePattern>()?.Expand();
		}

		#endregion
	}
}