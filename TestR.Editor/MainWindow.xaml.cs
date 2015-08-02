#region References

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using Newtonsoft.Json;
using TestR.Desktop;
using TestR.Editor.DragDropManagers;
using TestR.Editor.Extensions;
using TestR.Extensions;

#endregion

namespace TestR.Editor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		#region Fields

		private ListViewDragDropManager<ElementAction> _actionsDragDropManager;
		private readonly DispatcherTimer _dispatcherTimer;
		private TreeViewDragDropManager _elementsDragManager;
		private Element _focusedElement;
		private readonly Highlighter _highlighter;
		private Project _project;

		#endregion

		#region Constructors

		public MainWindow()
		{
			InitializeComponent();

			_dispatcherTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(250), DispatcherPriority.Normal, TimerControlDetectionTick, Dispatcher);
			_highlighter = new Highlighter();
			_project = new Project();

			DataContext = _project;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Window.Closed" /> event.
		/// </summary>
		/// <param name="e"> An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		protected override void OnClosed(EventArgs e)
		{
			_project?.Dispose();
			_project = null;
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
			base.OnClosing(e);
		}

		private void Actions_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var action = ((ListView) sender).SelectedItem as ElementAction;
			var element = _project.GetElement(action?.ApplicationId);
			if (element == null)
			{
				ElementDetails.Text = string.Empty;
				return;
			}

			_highlighter.SetElement(element);
			ElementDetails.Text = element.ToDetailString();
		}

		private void ActionsOnDrop(object sender, DragEventArgs dragEventArgs)
		{
			var elementReference = dragEventArgs.Data.GetData(typeof (ElementReference)) as ElementReference;
			if (dragEventArgs.Effects != DragDropEffects.Copy || elementReference == null)
			{
				return;
			}

			var element = _project.GetElement(elementReference.ApplicationId);
			var action = new ElementAction(element, ElementActionType.MoveMouseTo);
			_project.ElementActions.Add(action);
			Actions.SelectedItem = action;
		}

		private void AddToLog(string message)
		{
			Log.Text += message;
		}

		private void BuildTest(object sender, RoutedEventArgs e)
		{
			Code.Text = _project.Build();
			TabControl.SelectedIndex = 1;
		}

		private void DeleteAction(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			var action = button?.DataContext as ElementAction;
			_project.ElementActions.Remove(action);
		}

		private void ElementListOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			// Issue with trigger more than once.
			Dispatcher.BeginInvoke(DispatcherPriority.Normal, (NoArgDelegate) delegate
			{
				var reference = e.NewValue as ElementReference;
				var element = _project.GetElement(reference?.ApplicationId);
				if (element == null)
				{
					ElementDetails.Text = string.Empty;
					return;
				}

				_highlighter.SetElement(element);
				ElementDetails.Text = element.ToDetailString();
				Elements.Focus();
			});
		}

		private void Load(object sender, RoutedEventArgs e)
		{
			try
			{
				Progress.Visibility = Visibility.Visible;

				var dialog = new OpenFileDialog();
				dialog.DefaultExt = ".json";
				dialog.Filter = "JSON Files (*.json)|*.json";
				dialog.Multiselect = false;
				dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

				var result = dialog.ShowDialog(this);
				if (result.Value)
				{
					var data = File.ReadAllText(dialog.FileName);
					var project = JsonConvert.DeserializeObject<Project>(data);

					try
					{
						_project.Initialize(project);
						_project.Application.BringToFront();
						_project.RefreshElements();
					}
					catch (InvalidOperationException)
					{
						_project.Close();
					}
				}

				UpdateLayout();
			}
			finally
			{
				Progress.Visibility = Visibility.Hidden;
			}
		}

		private void Refresh(object sender, RoutedEventArgs e)
		{
			_project.RefreshElements();
		}

		private void RefreshElement(object sender, RoutedEventArgs routedEventArgs)
		{
			try
			{
				Progress.Visibility = Visibility.Visible;
				var menuItem = sender as MenuItem;
				var elementReference = menuItem?.DataContext as ElementReference;
				var element = _project.GetElement(elementReference?.ApplicationId);
				element?.UpdateChildren();
				elementReference?.Children.Clear();
				elementReference?.Children.AddRange(element?.Children.Select(x => new ElementReference(x)));
			}
			finally
			{
				Progress.Visibility = Visibility.Hidden;
			}
		}

		private void RunAction(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			var action = button?.DataContext as ElementAction;
			try
			{
				Progress.Visibility = Visibility.Visible;
				_project.RunAction(action);
			}
			catch (Exception ex)
			{
				AddToLog(ex.Message);
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				Progress.Visibility = Visibility.Hidden;
			}
		}

		private void RunTest(object sender, RoutedEventArgs e)
		{
			var currentCursor = Cursor;

			try
			{
				Cursor = Cursors.Wait;
				_highlighter.Visible = false;
				_project.RunTests();
			}
			catch (Exception ex)
			{
				AddToLog(ex.Message);
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				Cursor = currentCursor;
			}
		}

		private void Save(object sender, RoutedEventArgs e)
		{
			var data = JsonConvert.SerializeObject(_project);
			var dialog = new SaveFileDialog();
			dialog.DefaultExt = ".json";
			dialog.Filter = "JSON Files (*.json)|*.json";
			dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

			var result = dialog.ShowDialog(this);
			if (result.Value)
			{
				File.WriteAllText(dialog.FileName, data);
			}
		}

		private void SelectApplication(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog();
			dialog.DefaultExt = ".exe";
			dialog.Filter = "EXE Files (*.exe)|*.exe";
			dialog.Multiselect = false;
			dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

			var result = dialog.ShowDialog(this);
			if (result.Value)
			{
				Task.Run(() =>
				{
					try
					{
						_project.Initialize(dialog.FileName);
						_project.Application.BringToFront();
						Dispatcher.Invoke(() => { _project.RefreshElements(); });
					}
					catch (InvalidOperationException)
					{
						_project.Close();
					}
				});
			}
		}

		private void SelectProcess(object sender, RoutedEventArgs e)
		{
			var dialog = new ProcessWindow();
			dialog.Owner = this;

			var result = dialog.ShowDialog();
			if (result.HasValue && result.Value)
			{
				Task.Run(() =>
				{
					var process = dialog.SelectedProcess;

					try
					{
						_project.Initialize(process);
						_project.Application.BringToFront();
						Dispatcher.Invoke(() => { _project.RefreshElements(); });
					}
					catch (InvalidOperationException)
					{
						_project.Close();
					}
				});
			}
		}

		private void TimerControlDetectionTick(object sender, EventArgs e)
		{
			if (_project == null || (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl)))
			{
				return;
			}

			var foundElement = Element.FromCursor();
			foundElement.UpdateParents();
			foundElement = Element.GetFirstParentWithId(foundElement);

			// See if our element is already the focused element.
			if (_focusedElement != null && foundElement.ApplicationId == _focusedElement.ApplicationId)
			{
				return;
			}

			Debug.WriteLine("Found: " + foundElement.ApplicationId);
			if (_focusedElement != null)
			{
				Debug.WriteLine("Current: " + _focusedElement.ApplicationId);
			}

			// Find the element in our collection.
			var applicationElement = _project.GetElement(foundElement.ApplicationId);
			if (applicationElement == null)
			{
				var applicationElementParent = _project.GetElement(((Element) foundElement.Parent)?.ApplicationId);
				if (applicationElementParent != null)
				{
					applicationElementParent.UpdateChildren();
					applicationElement = _project.GetElement(foundElement.ApplicationId);
				}
			}

			if (applicationElement == null || applicationElement.ProcessId != _project.Application.Process.Id)
			{
				Debug.WriteLine("Could not find the application element...");
				return;
			}

			var currentElement = applicationElement;
			var elements = new List<Element>();

			while (currentElement != null)
			{
				elements.Add(currentElement);
				currentElement = currentElement.Parent as Element;
			}

			elements.Reverse();
			elements.ForEach(x => Elements.SelectItem(_project.GetElement(x.ApplicationId)));

			_focusedElement = applicationElement;
			Debug.WriteLine("Selected new focused element [" + _focusedElement.ApplicationId + "]...");
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			_elementsDragManager = new TreeViewDragDropManager(Elements);
			Elements.DragEnter += (o, args) => args.Effects = DragDropEffects.Move;
			_actionsDragDropManager = new ListViewDragDropManager<ElementAction>(Actions);
			Actions.DragEnter += (o, args) => args.Effects = DragDropEffects.Move;
			Actions.Drop += ActionsOnDrop;
		}

		#endregion

		#region Delegates

		private delegate void NoArgDelegate();

		#endregion
	}
}