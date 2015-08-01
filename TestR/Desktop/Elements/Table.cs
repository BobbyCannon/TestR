#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the table element.
	/// </summary>
	public class Table : Element
	{
		#region Constructors

		internal Table(IUIAutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion
	}
}