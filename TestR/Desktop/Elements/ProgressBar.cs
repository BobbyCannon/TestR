#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a progress bar element.
	/// </summary>
	public class ProgressBar : Element
	{
		#region Constructors

		internal ProgressBar(IUIAutomationElement element, Application application, Element parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}