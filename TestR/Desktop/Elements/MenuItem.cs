#region References

using System.Linq;
using TestR.Desktop.Automation;
using TestR.Desktop.Automation.Patterns;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the menu item for a window.
	/// </summary>
	public class MenuItem : Element
	{
		#region Fields

		private readonly ExpandCollapsePattern _expandCollapsePattern;

		#endregion

		#region Constructors

		internal MenuItem(AutomationElement element, IElementParent parent)
			: base(element, parent)
		{
			_expandCollapsePattern = GetPattern<ExpandCollapsePattern>(ExpandCollapsePattern.Pattern.Id);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a value indicating a sub menu is currently being shown.
		/// </summary>
		public bool SubMenuShown
		{
			get
			{
				return Children.MenuItems.Any(x => x.Visible);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this menu item supports expanding and collapsing pattern.
		/// </summary>
		public bool SupportsExpandingCollapsing
		{
			get { return _expandCollapsePattern != null; }
		}

		/// <summary>
		/// Gets the menu expand collapse state.
		/// </summary>
		public ExpandCollapseState ExpandCollapseState
		{
			get { return _expandCollapsePattern == null ? ExpandCollapseState.Collapsed : _expandCollapsePattern.Current.ExpandCollapseState; }
		}

		/// <summary>
		/// Gets the text value.
		/// </summary>
		public string Text
		{
			get { return Name; }
		}

		#endregion

		#region Methods

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

		#endregion
	}
}