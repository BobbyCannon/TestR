#region References

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using TestR.Desktop;
using TestR.Editor.DragDropManagers;
using TestR.Editor.Extensions;
using Keyboard = TestR.Native.Keyboard;
using Mouse = TestR.Native.Mouse;
using Point = System.Drawing.Point;

#endregion

namespace TestR.Editor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		#region Fields

		private ListViewDragDropManager _actionsDragDropManager;
		private readonly DispatcherTimer _dispatcherTimer;
		private TreeViewDragDropManager _elementsDragManager;
		private Element _focusedElement;
		private readonly ScreenBoundingRectangle _highlighter;
		private Project _project;

		#endregion

		#region Constructors

		public MainWindow()
		{
			InitializeComponent();

			_dispatcherTimer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, TimerControlDetectionTick, Dispatcher);
			_highlighter = new ScreenBoundingRectangle();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Window.Closed" /> event.
		/// </summary>
		/// <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		protected override void OnClosed(EventArgs e)
		{
			if (_project != null)
			{
				_project.Dispose();
				_project = null;
			}

			base.OnClosed(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Window.Closing" /> event.
		/// </summary>
		/// <param name="e"> A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data. </param>
		protected override void OnClosing(CancelEventArgs e)
		{
			_dispatcherTimer.Stop();
			_dispatcherTimer.Tick -= TimerControlDetectionTick;
			_highlighter.Dispose();
			Keyboard.StopMonitoring();
			Keyboard.KeyPressed -= KeyPressed;
			Mouse.StopMonitoring();
			Mouse.MouseChanged -= MouseChanged;
			base.OnClosing(e);
		}

		private void Actions_OnLostFocus(object sender, RoutedEventArgs e)
		{
			((ListView) sender).SelectedIndex = -1;
		}

		private void Actions_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var action = ((ListView) sender).SelectedItem as ElementAction;
			if (action == null)
			{
				return;
			}

			var element = _project.Elements.GetChild(action.ElementId);
			if (element != null)
			{
				_highlighter.Location = element.Location;
				_highlighter.Visible = true;
			}
			else
			{
				_highlighter.Visible = false;
			}
		}

		private void ActionsOnDrop(object sender, DragEventArgs dragEventArgs)
		{
			var data = dragEventArgs.Data.GetData("SelectedItem") as Element;
			if (dragEventArgs.Effects == DragDropEffects.Copy && data != null)
			{
				_project.ElementActions.Add(new ElementAction(data, ElementActionType.MoveMouseTo));
			}
		}

		private void ElementListOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			var element = e.NewValue as Element;
			if (element != null)
			{
				_highlighter.Location = element.Location;
				_highlighter.Visible = true;
			}
			else
			{
				_highlighter.Visible = false;
			}
		}

		private void ElementsOnLostFocus(object sender, RoutedEventArgs e)
		{
			//((TreeView) sender).ClearTreeViewSelection();
		}

		private void KeyPressed(Key key)
		{
			WriteLine(Log.Text = "Key Pressed: " + key);
		}

		private void MouseChanged(Mouse.MouseEvent mouseEvent, Point point)
		{
			Mouse.StopMonitoring();

			Task.Factory.StartNew(() =>
			{
				var element = Element.FromCursor();
				if (_focusedElement != element)
				{
					WriteLine("Click on " + element.Id + ":" + element.Name);
					_focusedElement = element;
				}
			});

			WriteLine("Mouse Changed: " + mouseEvent + " at " + point.X + ":" + point.Y);
		}

		private void Refresh(object sender, RoutedEventArgs e)
		{
			_project.RefreshElements();
		}

		private void RunTest(object sender, RoutedEventArgs e)
		{
			_project.RunTests();
		}

		private void SelectApplication(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog();
			dlg.DefaultExt = ".exe";
			dlg.Filter = "EXE Files (*.exe)|*.exe";
			dlg.Multiselect = false;
			dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

			var result = dlg.ShowDialog(this);
			if (result.Value)
			{
				if (_project != null)
				{
					_project.Dispose();
					_project = null;
				}

				_project = new Project(dlg.FileName);
				_project.RefreshElements();
				DataContext = _project;
			}
		}

		private void TimerControlDetectionTick(object sender, EventArgs e)
		{
			if (!System.Windows.Input.Keyboard.IsKeyDown(Key.LeftCtrl) && !System.Windows.Input.Keyboard.IsKeyDown(Key.RightCtrl))
			{
				return;
			}

			var element = Element.FromCursor();
			_highlighter.Location = element.Location;
			_highlighter.Visible = true;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			_elementsDragManager = new TreeViewDragDropManager(Elements);
			Elements.DragEnter += (o, args) => args.Effects = DragDropEffects.Move;
			_actionsDragDropManager = new ListViewDragDropManager(Actions);
			Actions.Drop += ActionsOnDrop;
		}

		private void WriteLine(string message)
		{
			Dispatcher.Invoke(() => { Log.Text = message + Environment.NewLine + Log.Text; }, DispatcherPriority.Background);
		}

		#endregion
	}
}