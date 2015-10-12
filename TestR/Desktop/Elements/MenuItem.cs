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
		public bool IsExpanded => ExpandCollapsePattern.Create(this)?.IsExpanded ?? false;

		/// <summary>
		/// Gets a value indicating whether this menu item supports expanding and collapsing pattern.
		/// </summary>
		public bool SupportsExpandingCollapsing => ExpandCollapsePattern.Create(this) != null;

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
		public override Element Click(int x = 0, int y = 0)
		{
			base.Click(x, y);
			UpdateChildren();
			return this;
		}

		/// <summary>
		/// Collapse the menu item.
		/// </summary>
		public MenuItem Collapse()
		{
			ExpandCollapsePattern.Create(this)?.Collapse();
			return this;
		}

		/// <summary>
		/// Expand the menu item.
		/// </summary>
		public MenuItem Expand()
		{
			ExpandCollapsePattern.Create(this)?.Expand();
			return this;
		}

		#endregion
	}
}