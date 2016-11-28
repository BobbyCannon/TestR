#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a split button element.
	/// </summary>
	public class SplitButton : DesktopElement
	{
		#region Constructors

		internal SplitButton(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}