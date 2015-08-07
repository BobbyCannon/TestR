#region References

#endregion

#region References

using TestR.Desktop.Pattern;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the menu item for a window.
	/// </summary>
	public class MenuItem : Element
	{
		#region Constructors

		internal MenuItem(IUIAutomationElement element, Application application, Element parent)
			: base(element, application, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the menu expand collapse state.
		/// </summary>
		public bool IsExpanded => ExpandCollapsePattern.New(this)?.IsExpanded ?? false;

		/// <summary>
		/// Gets a value indicating whether this menu item supports expanding and collapsing pattern.
		/// </summary>
		public bool SupportsExpandingCollapsing => ExpandCollapsePattern.New(this) != null;

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
			ExpandCollapsePattern.New(this)?.Collapse();
		}

		/// <summary>
		/// Expand the menu item.
		/// </summary>
		public void Expand()
		{
			ExpandCollapsePattern.New(this)?.Expand();
		}

		#endregion
	}
}