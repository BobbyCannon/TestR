#region References

using TestR.Desktop.Automation;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a document element.
	/// </summary>
	public class Document : Element
	{
		#region Constructors

		internal Document(AutomationElement element, IElementParent parent)
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