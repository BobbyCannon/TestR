#region References

using TestR.Desktop.Automation;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a button element.
	/// </summary>
	public class Button : Element
	{
		#region Constructors

		internal Button(AutomationElement element, IElementParent parent)
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