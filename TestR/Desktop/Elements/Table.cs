#region References

using TestR.Desktop.Automation;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the table element.
	/// </summary>
	public class Table : Element
	{
		#region Constructors

		internal Table(AutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion
	}
}