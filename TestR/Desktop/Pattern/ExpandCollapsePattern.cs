#region References

using System.Linq;
using TestR.Extensions;
using UIAutomationClient;

#endregion

namespace TestR.Desktop.Pattern
{
	public class ExpandCollapsePattern
	{
		private readonly IUIAutomationExpandCollapsePattern _pattern;

		#region Constructors

		private ExpandCollapsePattern(IUIAutomationExpandCollapsePattern pattern)
		{
			_pattern = pattern;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the current expanded state of the element.
		/// </summary>
		public ExpandCollapseState ExpandCollapseState => _pattern.CurrentExpandCollapseState.Convert();

		/// <summary>
		/// Gets the value indicating the element is expanded.
		/// </summary>
		public bool IsExpanded
		{
			get
			{
				var expandedStates = new[] { ExpandCollapseState.Expanded, ExpandCollapseState.PartiallyExpanded };
				return expandedStates.Contains(_pattern.CurrentExpandCollapseState.Convert());
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Collapses the element.
		/// </summary>
		public void Collapse()
		{
			_pattern.Collapse();
		}

		/// <summary>
		/// Expands the element.
		/// </summary>
		public void Expand()
		{
			_pattern.Expand();
		}

		public static ExpandCollapsePattern New(Element element)
		{
			var pattern = element.NativeElement.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId) as IUIAutomationExpandCollapsePattern;
			return pattern == null ? null : new ExpandCollapsePattern(pattern);
		}

		#endregion
	}
}