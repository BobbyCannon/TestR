#region References

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using Newtonsoft.Json;
using TestR.Desktop;

#endregion

namespace TestR.Editor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		#region Fields

		private readonly DispatcherTimer _dispatcherTimer;
		private readonly Highlighter _highlighter;
		private Project _project;
		private Element _lastAutoFocusedElement;

		#endregion

		#region Constructors

		public MainWindow()
		{
			InitializeComponent();

			_dispatcherTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(250), DispatcherPriority.Normal, TimerControlDetectionTick, Dispatcher);
			_highlighter = new Highlighter();
			_project = new Project();
			_project.Closed += ProjectOnClosed;

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

		private void AddFocusedElement(object sender, RoutedEventArgs e)
		{
			if (_project.FocusedElement == null)
			{
				return;
			}
			try
			{

				var action = new ElementAction(_project.FocusedElement, ElementActionType.MoveMouseTo);
				_project.ElementActions.Add(action);
				Actions.SelectedItem = action;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Failed to add focused element... " + ex.Message);
			}
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
					}
					catch (InvalidOperationException)
					{
						_project.Close();
					}
				}
			}
			finally
			{
				Progress.Visibility = Visibility.Hidden;
			}
		}

		private void ProjectOnClosed()
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Normal, (NoArgDelegate) delegate { Code.Clear(); });
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
			if (!result.Value)
			{
				return;
			}

			try
			{
				_project.Initialize(dialog.FileName);
				_project.Application.BringToFront();
			}
			catch (InvalidOperationException)
			{
				_project.Close();
			}
		}

		private void SelectProcess(object sender, RoutedEventArgs e)
		{
			var dialog = new ProcessWindow();
			dialog.Owner = this;

			var result = dialog.ShowDialog();
			if (!result.HasValue || !result.Value)
			{
				return;
			}

			var process = dialog.SelectedProcess;

			try
			{
				_project.Initialize(process);
				_project.Application.BringToFront();
			}
			catch (InvalidOperationException)
			{
				_project.Close();
			}
		}

		private void TimerControlDetectionTick(object sender, EventArgs e)
		{
			try
			{
				var foundElement = Element.FromFocusElement();
				foundElement?.UpdateParents();
				//foundElement?.UpdateChildren();
				var processId = GetFirstProcessId(foundElement);

				if (foundElement != null && processId == _project.ProcessId && foundElement.ApplicationId != _lastAutoFocusedElement?.ApplicationId)
				{
					_lastAutoFocusedElement = foundElement;
					_project.FocusedElement = _lastAutoFocusedElement;
					_highlighter.SetElement(_project.FocusedElement);
				}

				if (_project == null || (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl)))
				{
					return;
				}

				foundElement = Element.FromCursor();
				foundElement?.UpdateParents();
				//foundElement?.UpdateChildren();
				processId = GetFirstProcessId(foundElement);

				if (foundElement != null && processId == _project.ProcessId && foundElement.ApplicationId != _project.FocusedElement?.Id)
				{
					_project.FocusedElement = foundElement;
					_highlighter.SetElement(_project.FocusedElement);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Failed to get element... " + ex.Message);
			}
		}

		private int GetFirstProcessId(Element element)
		{
			if (element == null)
			{
				return 0;
			}

			return element.ProcessId != 0 
				? element.ProcessId 
				: GetFirstProcessId(element.Parent);
		}

		#endregion

		#region Delegates

		private delegate void NoArgDelegate();

		#endregion
	}
}