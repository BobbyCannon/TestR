#region References

using System.Windows.Controls;

#endregion

namespace TestR.Editor.Extensions
{
	public static class ListViewExtensions
	{
		#region Methods

		public static ListViewItem GetListViewItem(this ListView listView, int index)
		{
			return listView.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
		}

		public static int IndexUnderDragCursor(this ListView listView)
		{
			var index = -1;
			for (var i = 0; i < listView.Items.Count; ++i)
			{
				var item = listView.GetListViewItem(i);
				if (!item.IsMouseOver())
				{
					continue;
				}

				index = i;
				break;
			}

			return index;
		}

		#endregion
	}
}