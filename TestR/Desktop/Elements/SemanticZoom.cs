#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a semantic zoom element.
	/// </summary>
	public class SemanticZoom : Element
	{
		#region Constructors

		internal SemanticZoom(IUIAutomationElement element, Application application, Element parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}