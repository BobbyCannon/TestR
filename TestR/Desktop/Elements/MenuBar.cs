#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the menu bar for a window.
	/// </summary>
	public class MenuBar : Element
	{
		#region Constructors

		internal MenuBar(IUIAutomationElement element, Application application, Element parent)
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