#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the title bar for a window.
	/// </summary>
	public class TitleBar : Element
	{
		#region Constructors

		internal TitleBar(IUIAutomationElement element, Application application, Element parent)
			: base(element, application, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the close button.
		/// </summary>
		public Button CloseButton => Children.Get<Button>(x => x.Name == "Close");

		/// <summary>
		/// Gets the maximize button.
		/// </summary>
		public Button MaximizeButton => Children.Get<Button>(x => x.Name == "Maximize");

		/// <summary>
		/// Gets the maximize button.
		/// </summary>
		public Button MinimizeButton => Children.Get<Button>(x => x.Name == "Minimize");

		/// <summary>
		/// Gets the text value.
		/// </summary>
		public string Text => Name;

		#endregion
	}
}