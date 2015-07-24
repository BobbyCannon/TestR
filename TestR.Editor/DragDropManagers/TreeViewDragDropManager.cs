#region References

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

#endregion

namespace TestR.Editor.DragDropManagers
{
	public class TreeViewDragDropManager
	{
		#region Fields

		private bool _isDragging;
		private Point _startPoint;
		private readonly TreeView _treeView;

		#endregion

		#region Constructors

		public TreeViewDragDropManager(TreeView treeView)
		{
			_treeView = treeView;
			_treeView.PreviewMouseLeftButtonDown += PreviewMouseLeftButtonDown;
			_treeView.PreviewMouseMove += PreviewMouseMove;
			_startPoint = new Point();
		}

		#endregion

		#region Methods

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
			if (_treeView.SelectedItem == null)
			{
				return;
			}

			_isDragging = true;
			DragDrop.DoDragDrop(_treeView, _treeView.SelectedItem, DragDropEffects.Copy);
			_isDragging = false;
		}

		#endregion
	}
}