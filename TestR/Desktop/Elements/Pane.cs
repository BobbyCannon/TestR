#region References

#endregion

#region References

using TestR.Desktop.Automation;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a edit element.
	/// </summary>
	public class Pane : Element
	{
		#region Constructors

		internal Pane(AutomationElement element, IElementParent parent)
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
			set { SetText(value); }
		}

		#endregion
	}
}