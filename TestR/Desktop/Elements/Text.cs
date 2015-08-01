#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the text for a window.
	/// </summary>
	public class Text : Element
	{
		#region Constructors

		internal Text(IUIAutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion
	}
}