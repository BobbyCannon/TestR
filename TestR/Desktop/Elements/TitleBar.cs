#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the title bar for a window.
	/// </summary>
	public class TitleBar : DesktopElement
	{
		#region Constructors

		internal TitleBar(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the close button.
		/// </summary>
		public Button CloseButton => Children.First<Button>(x => x.Name == "Close");

		/// <summary>
		/// Gets the maximize button.
		/// </summary>
		public Button MaximizeButton => Children.First<Button>(x => x.Name == "Maximize");

		/// <summary>
		/// Gets the maximize button.
		/// </summary>
		public Button MinimizeButton => Children.First<Button>(x => x.Name == "Minimize");

		#endregion
	}
}