#region References

using System.Linq;
using TestR.Extensions;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Pattern
{
	public class ExpandCollapsePattern : BasePattern
	{
		#region Constructors

		private ExpandCollapsePattern(Element element)
			: base(element)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the current expanded state of the element.
		/// </summary>
		public ExpandCollapseState ExpandCollapseState => GetPattern<IUIAutomationExpandCollapsePattern>()?.CurrentExpandCollapseState.Convert() ?? ExpandCollapseState.Expanded;

		/// <summary>
		/// Gets the value indicating the element is expanded.
		/// </summary>
		public bool IsExpanded
		{
			get
			{
				var expandedStates = new[] { ExpandCollapseState.Expanded, ExpandCollapseState.PartiallyExpanded };
				return expandedStates.Contains(GetPattern<IUIAutomationExpandCollapsePattern>()?.CurrentExpandCollapseState.Convert() ?? ExpandCollapseState.Collapsed);
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Collapses the element.
		/// </summary>
		public void Collapse()
		{
			GetPattern<IUIAutomationExpandCollapsePattern>()?.Collapse();
		}

		/// <summary>
		/// Expands the element.
		/// </summary>
		public void Expand()
		{
			GetPattern<IUIAutomationExpandCollapsePattern>()?.Expand();
		}

		public static ExpandCollapsePattern New(Element element)
		{
			return new ExpandCollapsePattern(element);
		}

		#endregion
	}
}