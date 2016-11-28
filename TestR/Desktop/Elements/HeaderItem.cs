#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the header item.
	/// </summary>
	public class HeaderItem : DesktopElement
	{
		#region Constructors

		internal HeaderItem(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}