#region References

using TestR.Desktop.Automation;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the text for a window.
	/// </summary>
	public class Text : Element
	{
		#region Constructors

		internal Text(AutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion
	}
}