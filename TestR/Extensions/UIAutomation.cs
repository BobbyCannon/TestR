#region References

using System;
using UIAutomationClient;
using ExpandCollapseState = TestR.Desktop.Pattern.ExpandCollapseState;
using ToggleState = TestR.Desktop.Pattern.ToggleState;

#endregion

namespace TestR.Extensions
{
	public static partial class Helper
	{
		#region Methods

		internal static ToggleState Convert(this UIAutomationClient.ToggleState state)
		{
			switch (state)
			{
				case UIAutomationClient.ToggleState.ToggleState_Off:
					return ToggleState.Off;

				case UIAutomationClient.ToggleState.ToggleState_On:
					return ToggleState.On;

				case UIAutomationClient.ToggleState.ToggleState_Indeterminate:
					return ToggleState.Indeterminate;

				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}
		}

		internal static ExpandCollapseState Convert(this UIAutomationClient.ExpandCollapseState state)
		{
			switch (state)
			{
				case UIAutomationClient.ExpandCollapseState.ExpandCollapseState_Collapsed:
					return ExpandCollapseState.Collapsed;

				case UIAutomationClient.ExpandCollapseState.ExpandCollapseState_Expanded:
					return ExpandCollapseState.Expanded;

				case UIAutomationClient.ExpandCollapseState.ExpandCollapseState_PartiallyExpanded:
					return ExpandCollapseState.PartiallyExpanded;

				case UIAutomationClient.ExpandCollapseState.ExpandCollapseState_LeafNode:
					return ExpandCollapseState.LeafNode;

				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}
		}

		internal static IUIAutomationElement GetCurrentParent(this IUIAutomationElement element)
		{
			var automation = new CUIAutomationClass();
			var walker = automation.CreateTreeWalker(automation.RawViewCondition);
			return walker.GetParentElement(element);
		}

		#endregion
	}
}