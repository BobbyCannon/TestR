#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a edit element.
	/// </summary>
	public class Edit : Element
	{
		#region Constructors

		internal Edit(IUIAutomationElement element, IElementParent parent)
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