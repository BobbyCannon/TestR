#region References

using UIAutomationClient;

#endregion

namespace TestR.Desktop.Elements
{
	/// <summary>
	/// Represents a datagrid element.
	/// </summary>
	public class DataGrid : Element
	{
		#region Constructors

		internal DataGrid(IUIAutomationElement element, Application application, Element parent)
			: base(element, application, parent)
		{
		}

		#endregion
	}
}