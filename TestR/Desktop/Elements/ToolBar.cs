#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the tool bar for a window.
	/// </summary>
	public class ToolBar : DesktopElement
	{
		#region Constructors

		internal ToolBar(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}