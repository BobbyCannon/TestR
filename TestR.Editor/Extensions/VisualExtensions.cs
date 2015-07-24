#region References

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		/// Run an action on each item in the tree view.
		/// </summary>
		/// <param name="treeView"> The treeview to expand all nodes. </param>
		/// <param name="action"> The action to process the tree view item. </param>
		public static void ForEach(this TreeView treeView, Action<TreeViewItem> action)
		{
			if (treeView == null)
			{
				return;
			}

			ProcessTreeViewItemsControlSelection(treeView.Items, treeView.ItemContainerGenerator, action);
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

		public static bool IsMouseOver(this Visual target)
		{
			// We need to use MouseUtilities to figure out the cursor
			// coordinates because, during a drag-drop operation, the WPF
			// mechanisms for getting the coordinates behave strangely.

			var bounds = VisualTreeHelper.GetDescendantBounds(target);
			var mousePos = target.GetMousePosition();
			return bounds.Contains(mousePos);
		}

		public static void SelectItem(this TreeView treeView, object item)
		{
			var items = GetDescendants<TreeViewItem>(treeView).ToList();
			var treeViewItem = items.FirstOrDefault(tvi => tvi.DataContext == item);
			if (treeViewItem == null)
			{
				return;
			}

			treeViewItem.IsSelected = true;
			treeViewItem.IsExpanded = true;
			treeView.UpdateLayout();
			treeViewItem.Focus();
		}

		private static IEnumerable<T> GetDescendants<T>(DependencyObject parent)
			where T : DependencyObject
		{
			var count = VisualTreeHelper.GetChildrenCount(parent);
			for (var i = 0; i < count; ++i)
			{
				// Obtain the child
				var child = VisualTreeHelper.GetChild(parent, i);
				if (child is T)
				{
					yield return (T) child;
				}

				// Return all the descendant children
				foreach (var subItem in GetDescendants<T>(child))
				{
					yield return subItem;
				}
			}
		}

		private static void ProcessTreeViewItemsControlSelection(ICollection collection, ItemContainerGenerator itemContainerGenerator, Action<TreeViewItem> action)
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

				action(treeViewItem);
				ProcessTreeViewItemsControlSelection(treeViewItem.Items, treeViewItem.ItemContainerGenerator, action);
			}
		}

		#endregion
	}
}