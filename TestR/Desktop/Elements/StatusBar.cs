#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the status bar for a window.
	/// </summary>
	public class StatusBar : DesktopElement
	{
		#region Constructors

		internal StatusBar(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}