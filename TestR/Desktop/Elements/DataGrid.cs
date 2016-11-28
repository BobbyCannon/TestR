#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a datagrid element.
	/// </summary>
	public class DataGrid : DesktopElement
	{
		#region Constructors

		internal DataGrid(IUIAutomationElement element, Application application, DesktopElement parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}