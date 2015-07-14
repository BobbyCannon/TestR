#region References

using TestR.Desktop.Automation;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a list item element.
	/// </summary>
	public class ListItem : Element
	{
		#region Constructors

		internal ListItem(AutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion
	}
}