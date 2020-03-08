#region References

using Interop.UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a semantic zoom element.
	/// </summary>
	public class SemanticZoom : DesktopElement
	{
		#region Constructors

		internal SemanticZoom(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}