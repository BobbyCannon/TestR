#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a document element.
	/// </summary>
	public class Document : DesktopElement
	{
		#region Constructors

		internal Document(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the text value.
		/// </summary>
		public string Text
		{
			get { return GetText(); }
			set { SetText(value); }
		}

		#endregion
	}
}