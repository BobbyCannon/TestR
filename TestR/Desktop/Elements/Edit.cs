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
		/// Gets a value indicating whether the control can have a value set programmatically, or that can be edited by the user.
		/// </summary>
		public bool ReadOnly => GetPattern<IUIAutomationValuePattern>()?.CurrentIsReadOnly == 1;

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