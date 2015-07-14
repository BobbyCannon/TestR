#region References

using TestR.Desktop.Automation;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a custom element.
	/// </summary>
	public class Custom : Element
	{
		#region Constructors

		internal Custom(AutomationElement element, IElementParent parent)
			: base(element, parent)
		{
		}

		#endregion
	}
}