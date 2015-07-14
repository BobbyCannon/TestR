#region References

using TestR.Desktop.Automation;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a hyperlink element.
	/// </summary>
	public class Hyperlink : Element
	{
		#region Constructors

		internal Hyperlink(AutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion
	}
}