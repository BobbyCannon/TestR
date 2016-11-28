#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a semantic zoom element.
	/// </summary>
	public class Slider : DesktopElement
	{
		#region Constructors

		internal Slider(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}