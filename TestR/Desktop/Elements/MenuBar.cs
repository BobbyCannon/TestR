#region References

using TestR.Desktop.Automation;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the menu bar for a window.
	/// </summary>
	public class MenuBar : Element
	{
		#region Constructors

		internal MenuBar(AutomationElement element, IElementParent parent)
			: base(element, parent)
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