#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a hyperlink element.
	/// </summary>
	public class Hyperlink : DesktopElement
	{
		#region Constructors

		internal Hyperlink(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}