#region References

using UIAutomationClient;

#endregion

namespace TestR.Extensions
{
	public static partial class Helper
	{
		#region Methods

		internal static IUIAutomationElement GetCurrentParent(this IUIAutomationElement element)
		{
			var automation = new CUIAutomationClass();
			var walker = automation.CreateTreeWalker(automation.RawViewCondition);
			return walker.GetParentElement(element);
		}

		#endregion
	}
}