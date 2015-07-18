#region References

using System.Windows.Controls;

#endregion

namespace TestR.Editor.DragDropManagers
{
	public class ListViewDragDropManager
	{
		private readonly ListView _listView;

		#region Constructors

		public ListViewDragDropManager(ListView listView)
		{
			_listView = listView;
		}

		#endregion
	}
}