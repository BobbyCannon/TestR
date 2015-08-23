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
			// There is a issue in Windows 10 and Cortana (or modern apps) where there is a 60 second delay when walking the root element. 
			// When you hit the last sibling it delays. For now we are simply going to return the main window and we'll roll this code 
			// back once the Windows 10 issue has been resolved.

			process.Refresh();
			var automation = new CUIAutomationClass();
			return new[] { automation.ElementFromHandle(process.MainWindowHandle) };
		}

		#endregion
	}
}