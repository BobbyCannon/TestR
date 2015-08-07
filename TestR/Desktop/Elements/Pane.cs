#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a edit element.
	/// </summary>
	public class Pane : Element
	{
		#region Constructors

		internal Pane(IUIAutomationElement element, Application application, Element parent)
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
			get { return Name; }
			set { SetText(value); }
		}

		#endregion
	}
}