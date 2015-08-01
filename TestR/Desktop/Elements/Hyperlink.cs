#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a hyperlink element.
	/// </summary>
	public class Hyperlink : Element
	{
		#region Constructors

		internal Hyperlink(IUIAutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion
	}
}