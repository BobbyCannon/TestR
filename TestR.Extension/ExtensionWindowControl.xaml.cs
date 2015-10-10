#region References

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using TestR.Desktop;
using TestR.Native;

#endregion

namespace TestR.Extension
{
	/// <summary>
	/// Interaction logic for ExtensionWindowControl.
	/// </summary>
	public partial class ExtensionWindowControl
	{
		#region Fields

		private readonly Project _project;
		private readonly BackgroundWorker _worker;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtensionWindowControl" /> class.
		/// </summary>
		public ExtensionWindowControl()
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

		private void ProjectOnClosed()
		{
			_worker.CancelAsync();
		}

		private void SelectApplication(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog();
			dialog.DefaultExt = ".exe";
			dialog.Filter = "EXE Files (*.exe)|*.exe";
			dialog.Multiselect = false;
			dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

			var result = dialog.ShowDialog();
			if (!result.HasValue || !result.Value)
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
			dialog.Owner = (Window) Parent;

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