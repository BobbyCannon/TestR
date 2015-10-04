#region References

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
using TestR.Desktop;
using TestR.Helpers;
using Keyboard = TestR.Native.Keyboard;

#endregion

namespace TestR.Editor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		#region Fields

		private Project _project;
		private readonly BackgroundWorker _worker;

		#endregion

		#region Constructors

		public MainWindow()
		{
			InitializeComponent();

			_worker = new BackgroundWorker();
			_worker.WorkerReportsProgress = true;
			_worker.WorkerSupportsCancellation = true;
			_worker.DoWork += WorkerOnDoWork;
			_worker.ProgressChanged += WorkerOnProgressChanged;
			_project = new Project(Dispatcher);
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
			_worker.CancelAsync();
			_project.Dispose();
			Utility.Wait(() => !_worker.IsBusy);
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

		private void FocusChildrenSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count <= 0)
			{
				return;
			}

			var child = e.AddedItems[0] as Element;
			if (child != null)
			{
				child.UpdateChildren();
				_project.FocusedElement = child;
			}
		}

		private static int GetFirstProcessId(Element element)
		{
			if (element == null)
			{
				return 0;
			}

			return element.ProcessId != 0
				? element.ProcessId
				: GetFirstProcessId(element.Parent);
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
						_worker.RunWorkerAsync(_project);
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
			_worker.CancelAsync();
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
				_worker.RunWorkerAsync(_project);
			}
			catch (InvalidOperationException)
			{
				_project.Close();
			}
		}

		private void SelectParentElement(object sender, RoutedEventArgs e)
		{
			var parent = _project.FocusedElement?.Parent;
			if (parent != null)
			{
				parent.UpdateChildren();
				_project.FocusedElement = parent;
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
				_worker.RunWorkerAsync(_project);
			}
			catch (InvalidOperationException)
			{
				_project.Close();
			}
		}

		private void UpdateFocusedElementChildren(object sender, RoutedEventArgs e)
		{
			_project?.FocusedElement?.UpdateChildren();
		}

		private static void WorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
		{
			var worker = (BackgroundWorker) sender;
			Element lastAutoFocusedElement = null;
			var project = (Project) doWorkEventArgs.Argument;

			while (!worker.CancellationPending)
			{
				try
				{
					var foundElement = Element.FromFocusElement();
					foundElement?.UpdateParents();
					var processId = GetFirstProcessId(foundElement);

					if (foundElement != null && processId == project.ProcessId && foundElement.ApplicationId != lastAutoFocusedElement?.ApplicationId)
					{
						lastAutoFocusedElement = foundElement;
						worker.ReportProgress(0, foundElement);
					}

					if (!Keyboard.IsControlPressed())
					{
						Thread.Sleep(250);
						continue;
					}

					foundElement = Element.FromCursor();
					foundElement?.UpdateParents();
					processId = GetFirstProcessId(foundElement);

					if (foundElement != null && processId == project.ProcessId && foundElement.ApplicationId != project.FocusedElement?.Id)
					{
						worker.ReportProgress(0, foundElement);
					}

					Thread.Sleep(250);
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Failed to get element... " + ex.Message);
				}
			}
		}

		private void WorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
		{
			var foundElement = (Element) progressChangedEventArgs.UserState;
			var applicationElement = _project.Application.Get(foundElement?.ApplicationId) ?? foundElement;

			if (_project.FocusedElement == applicationElement)
			{
				return;
			}
			
			_project.FocusedElement = applicationElement;
		}

		#endregion
	}
}