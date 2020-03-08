#region References

using Interop.UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a tooltip element.
	/// </summary>
	public class ToolTip : DesktopElement
	{
		#region Constructors

		internal ToolTip(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}