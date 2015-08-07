#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a list item element.
	/// </summary>
	public class ListItem : Element
	{
		#region Constructors

		internal ListItem(IUIAutomationElement element, Application application, Element parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}