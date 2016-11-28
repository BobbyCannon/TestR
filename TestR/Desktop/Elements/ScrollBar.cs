#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the scroll bar for a window.
	/// </summary>
	public class ScrollBar : DesktopElement
	{
		#region Constructors

		internal ScrollBar(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}