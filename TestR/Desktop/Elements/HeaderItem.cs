#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the header item.
	/// </summary>
	public class HeaderItem : Element
	{
		#region Constructors

		internal HeaderItem(IUIAutomationElement element, Application application, Element parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}