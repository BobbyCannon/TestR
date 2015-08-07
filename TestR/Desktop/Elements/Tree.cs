#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the tree for a window.
	/// </summary>
	public class Tree : Element
	{
		#region Constructors

		internal Tree(IUIAutomationElement element, Application application, Element parent)
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