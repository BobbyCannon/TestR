#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a radio button element.
	/// </summary>
	public class RadioButton : Element
	{
		#region Constructors

		internal RadioButton(IUIAutomationElement element, Application application, Element parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}