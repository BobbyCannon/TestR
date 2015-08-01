#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a combo box element.
	/// </summary>
	public class ComboBox : Element
	{
		#region Constructors

		internal ComboBox(IUIAutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion
	}
}