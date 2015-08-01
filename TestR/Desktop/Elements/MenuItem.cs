#region References

#endregion

#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the menu item for a window.
	/// </summary>
	public class MenuItem : Element
	{
		#region Fields

		private readonly IUIAutomationExpandCollapsePattern _expandCollapsePattern;

		#endregion

		#region Constructors

		internal MenuItem(IUIAutomationElement element, IElementParent parent)
			: base(element, parent)
		{
			_expandCollapsePattern = NativeElement.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId) as IUIAutomationExpandCollapsePattern;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the menu expand collapse state.
		/// </summary>
		public bool IsExpanded => SupportsExpandingCollapsing && _expandCollapsePattern.CurrentExpandCollapseState != ExpandCollapseState.ExpandCollapseState_Collapsed;

		/// <summary>
		/// Gets a value indicating whether this menu item supports expanding and collapsing pattern.
		/// </summary>
		public bool SupportsExpandingCollapsing
		{
			get { return _expandCollapsePattern != null; }
		}

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
			_expandCollapsePattern.Collapse();
		}

		/// <summary>
		/// Expand the menu item.
		/// </summary>
		public void Expand()
		{
			_expandCollapsePattern.Expand();
		}

		#endregion
	}
}