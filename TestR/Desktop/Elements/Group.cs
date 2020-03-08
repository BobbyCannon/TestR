#region References

using Interop.UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents the group for a window.
	/// </summary>
	public class Group : DesktopElement
	{
		#region Constructors

		internal Group(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}