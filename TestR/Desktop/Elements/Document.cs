#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a document element.
	/// </summary>
	public class Document : Element
	{
		#region Constructors

		internal Document(IUIAutomationElement element, IElementParent parent)
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
			get { return GetText(); }
			set { SetText(value); }
		}

		#endregion
	}
}