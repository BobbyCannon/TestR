#region References

using System.Collections.Generic;
using System.Diagnostics;
using UIAutomationClient;

#endregion

namespace TestR.Extensions
{
	public static partial class Helper
	{
		#region Methods

		internal static IEnumerable<IUIAutomationElement> GetWindows(this Process process)
		{
			var automation = new CUIAutomationClass();
			var walker = automation.CreateTreeWalker(automation.RawViewCondition);
			var child = walker.GetFirstChildElement(automation.GetRootElement());

			while (child != null)
			{
				if (child.CurrentProcessId == process.Id && child.CurrentControlType == UIA_ControlTypeIds.UIA_WindowControlTypeId)
				{
					yield return child;
				}

				child = walker.GetNextSiblingElement(child);
			}
		}

		#endregion
	}
}