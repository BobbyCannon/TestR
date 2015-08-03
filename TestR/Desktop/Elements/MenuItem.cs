#region References

#endregion

#region References

using TestR.Extensions;
using UIAutomationClient;
using ExpandCollapseState = TestR.Desktop.Pattern.ExpandCollapseState;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the menu item for a window.
	/// </summary>
	public class MenuItem : Element
	{
		#region Constructors

		internal MenuItem(IUIAutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the menu expand collapse state.
		/// </summary>
		public bool IsExpanded => SupportsExpandingCollapsing && GetPattern<IUIAutomationExpandCollapsePattern>()?.CurrentExpandCollapseState.Convert() != ExpandCollapseState.Collapsed;

		/// <summary>
		/// Gets a value indicating whether this menu item supports expanding and collapsing pattern.
		/// </summary>
		public bool SupportsExpandingCollapsing => GetPattern<IUIAutomationExpandCollapsePattern>() != null;

		/// <summary>
		/// Gets the text value.
		/// </summary>
		public string Text => Name;

		#endregion

		#region Methods

		/// <summary>
		/// Performs mouse click at the center of the element.
		/// </summary>
		/// <param name="x"> Optional X offset when clicking. </param>
		/// <param name="y"> Optional Y offset when clicking. </param>
		public override void Click(int x = 0, int y = 0)
		{
			base.Click(x, y);
			UpdateChildren();
		}

		/// <summary>
		/// Collapse the menu item.
		/// </summary>
		public void Collapse()
		{
			GetPattern<IUIAutomationExpandCollapsePattern>()?.Collapse();
		}

		/// <summary>
		/// Expand the menu item.
		/// </summary>
		public void Expand()
		{
			GetPattern<IUIAutomationExpandCollapsePattern>()?.Expand();
		}

		#endregion
	}
}