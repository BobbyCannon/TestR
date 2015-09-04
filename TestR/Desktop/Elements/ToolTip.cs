#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a tooltip element.
	/// </summary>
	public class ToolTip : Element
	{
		#region Constructors

		internal ToolTip(IUIAutomationElement element, Application application, Element parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}