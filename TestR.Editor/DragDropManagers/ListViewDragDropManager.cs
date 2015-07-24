#region References

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using TestR.Editor.Extensions;

#endregion

namespace TestR.Editor.DragDropManagers
{
	public class ListViewDragDropManager<T> where T : class
	{
		#region Fields

		private bool _isDragging;
		private readonly ListView _listView;
		private Point _startPoint;

		#endregion

		#region Constructors

		public ListViewDragDropManager(ListView listView)
		{
			_listView = listView;
			_listView.PreviewMouseLeftButtonDown += PreviewMouseLeftButtonDown;
			_listView.PreviewMouseMove += PreviewMouseMove;
			_listView.Drop += Drop;
			_startPoint = new Point();
		}

		#endregion

		#region Methods

		private void Drop(object sender, DragEventArgs e)
		{
			if (!e.Data.GetDataPresent(typeof (T)))
			{
				return;
			}

			// Get the data object which was dropped.
			var data = e.Data.GetData(typeof (T)) as T;
			if (data == null)
			{
				return;
			}

			// Get the ObservableCollection<ItemType> which contains the dropped data object.
			var itemsSource = _listView.ItemsSource as ObservableCollection<T>;
			if (itemsSource == null)
			{
				throw new Exception("A ListView managed by ListViewDragManager must have its ItemsSource set to an ObservableCollection<T>.");
			}

			var oldIndex = itemsSource.IndexOf(data);
			var newIndex = _listView.IndexUnderDragCursor();

			if (newIndex < 0)
			{
				if (itemsSource.Count == 0)
				{
					// The drag started somewhere else, and our ListView is empty so make the new item the first in the list.
					newIndex = 0;
				}
				else if (oldIndex < 0)
				{
					// The drag started somewhere else, but our ListView has items so make the new item the last in the list.
					newIndex = itemsSource.Count;
				}
				else
				{
					// The user is trying to drop an item from our ListView into our ListView, but the mouse is not over an item, so don't let them drop it.
					return;
				}
			}

			// Dropping an item back onto itself is not considered an actual 'drop'.
			if (oldIndex == newIndex)
			{
				return;
			}

			// Move the dragged data object from it's original index to the new index (according to where the mouse cursor is).  If it was
			// not previously in the ListBox, then insert the item.
			if (oldIndex > -1)
			{
				itemsSource.Move(oldIndex, newIndex);
				// Set the Effects property so that the call to DoDragDrop will return 'Move'.
				e.Effects = DragDropEffects.Move;
			}
			else
			{
				itemsSource.Insert(newIndex, data);
				e.Effects = DragDropEffects.Copy;
			}
		}

		private static bool IsMouseOverScrollbar(object sender, Point mousePosition)
		{
			if (!(sender is Visual))
			{
				return false;
			}

			var hit = VisualTreeHelper.HitTest((Visual) sender, mousePosition);
			if (hit == null)
			{
				return false;
			}

			var visualHit = hit.VisualHit;
			while (visualHit != null)
			{
				if (visualHit is ScrollBar)
				{
					return true;
				}

				if ((visualHit is Visual) || (visualHit is Visual3D))
				{
					visualHit = VisualTreeHelper.GetParent(visualHit);
				}
				else
				{
					visualHit = LogicalTreeHelper.GetParent(visualHit);
				}
			}

			return false;
		}

		private void PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			_startPoint = e.GetPosition(null);
		}

		private void PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton != MouseButtonState.Pressed && (e.RightButton != MouseButtonState.Pressed || _isDragging))
			{
				return;
			}

			if (IsMouseOverScrollbar(sender, e.GetPosition(sender as IInputElement)))
			{
				return;
			}

			var position = e.GetPosition(null);
			if (Math.Abs(position.X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(position.Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
			{
				StartDrag(e);
			}
		}

		private void StartDrag(MouseEventArgs e)
		{
			if (_listView.SelectedItem == null)
			{
				return;
			}

			_isDragging = true;
			DragDrop.DoDragDrop(_listView, _listView.SelectedItem, DragDropEffects.Copy);
			_isDragging = false;
		}

		#endregion
	}
}