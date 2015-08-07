#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the tool bar for a window.
	/// </summary>
	public class ToolBar : Element
	{
		#region Constructors

		internal ToolBar(IUIAutomationElement element, Application application, Element parent)
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
		}

		#endregion
	}
}