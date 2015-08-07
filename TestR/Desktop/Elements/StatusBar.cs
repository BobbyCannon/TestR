#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the status bar for a window.
	/// </summary>
	public class StatusBar : Element
	{
		#region Constructors

		internal StatusBar(IUIAutomationElement element, Application application, Element parent)
			: base(element, application, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the text value.
		/// </summary>
		public string Text => Name;

		#endregion
	}
}