#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a edit element.
	/// </summary>
	public class Pane : DesktopElement
	{
		#region Constructors

		internal Pane(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}