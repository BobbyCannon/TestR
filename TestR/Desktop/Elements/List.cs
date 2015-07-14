#region References

using TestR.Desktop.Automation;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the list for a window.
	/// </summary>
	public class List : Element
	{
		#region Constructors

		internal List(AutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion
	}
}