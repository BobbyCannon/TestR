#region References

using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TestR.Native;

#endregion

namespace TestR.Editor.Extensions
{
	public static class VisualExtensions
	{
		#region Methods

		/// <summary>
		/// Clears all selections from the treeview.
		/// </summary>
		/// <param name="treeView"> The treeview to clear selections from. </param>
		public static void ClearTreeViewSelection(this TreeView treeView)
		{
			if (treeView == null)
			{
				return;
			}

			ClearTreeViewItemsControlSelection(treeView.Items, treeView.ItemContainerGenerator);
		}

		/// <summary>
		/// Returns the mouse cursor location.  This method is necessary during
		/// a drag-drop operation because the WPF mechanisms for retrieving the
		/// cursor coordinates are unreliable.
		/// </summary>
		/// <param name="relativeTo"> The Visual to which the mouse coordinates will be relative. </param>
		public static Point GetMousePosition(this Visual relativeTo)
		{
			var mouse = Mouse.GetCursorPosition();
			return relativeTo.PointFromScreen(new Point(mouse.X, mouse.Y));
		}

		private static void ClearTreeViewItemsControlSelection(ICollection collection, ItemContainerGenerator itemContainerGenerator)
		{
			if ((collection == null) || (itemContainerGenerator == null))
			{
				return;
			}

			for (var i = 0; i < collection.Count; i++)
			{
				var treeViewItem = itemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
				if (treeViewItem == null)
				{
					continue;
				}

				ClearTreeViewItemsControlSelection(treeViewItem.Items, treeViewItem.ItemContainerGenerator);
				treeViewItem.IsSelected = false;
			}
		}

		#endregion
	}
}