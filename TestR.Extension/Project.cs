#region References

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Threading;
using Newtonsoft.Json;
using TestR.Desktop;
using Application = TestR.Desktop.Application;

#endregion

namespace TestR.Extension
{
	public sealed class Project : IDisposable, INotifyPropertyChanged
	{
		#region Fields

		private ObservableCollection<ElementAction> _actions;
		private Application _application;
		private string _applicationFilePath;
		private readonly Dispatcher _dispatcher;
		private Element _focusedElement;
		private readonly Highlighter _highlighter;

		#endregion

		#region Constructors

		public Project(Dispatcher dispatcher)
		{
			_actions = new ObservableCollection<ElementAction>();
			_application = null;
			_dispatcher = dispatcher;
			_highlighter = new Highlighter();
			ApplicationFilePath = string.Empty;
		}

		#endregion

		#region Properties

		[JsonIgnore]
		public Application Application
		{
			get { return _application; }
			private set
			{
				_application = value;
				OnPropertyChanged(nameof(Application));
				OnPropertyChanged(nameof(IsLoaded));
			}
		}

		public string ApplicationFilePath
		{
			get { return _applicationFilePath; }
			set
			{
				_applicationFilePath = value;
				OnPropertyChanged(nameof(ApplicationFilePath));
			}
		}

		public ObservableCollection<ElementAction> ElementActions
		{
			get { return _actions; }
			set
			{
				_actions = value;
				OnPropertyChanged(nameof(ElementActions));
			}
		}

		[JsonIgnore]
		public Element FocusedElement
		{
			get { return _focusedElement; }
			internal set
			{
				_focusedElement = value;
				_highlighter.SetElement(value);
				OnPropertyChanged(nameof(FocusedElement));
				OnPropertyChanged(nameof(FocusedElementHasParent));
				OnPropertyChanged(nameof(FocusedElementHasChildren));
				OnPropertyChanged(nameof(IsElementFocused));
				OnPropertyChanged(nameof(FocusedElementDetails));
			}
		}

		public string FocusedElementDetails => FocusedElement?.ToDetailString() ?? string.Empty;

		public bool FocusedElementHasChildren => FocusedElement?.Children?.Count > 0;

		public bool FocusedElementHasParent => FocusedElement?.Parent != null;

		public bool IsElementFocused => FocusedElement != null;

		/// <summary>
		/// Returns true if the application is loaded.
		/// </summary>
		public bool IsLoaded => Application != null;

		public int ProcessId => Application?.Process?.Id ?? 0;
		
		#endregion

		#region Methods

		public void Close()
		{
			OnClosed();

			if (Application != null)
			{
				_dispatcher.Invoke(() =>
				{
					ElementActions.Clear();
					FocusedElement = null;
				});

				ApplicationFilePath = string.Empty;
				Application.Closed -= Close;
				Application.Dispose();
				Application = null;
			}

			OnPropertyChanged(nameof(IsLoaded));
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Initialize(string applicationPath)
		{
			Close();

			ApplicationFilePath = applicationPath;
			Application = Application.AttachOrCreate(ApplicationFilePath);
			Application.Closed += Close;
			Application.Timeout = TimeSpan.FromSeconds(5);

			OnPropertyChanged(nameof(Application));
			OnPropertyChanged(nameof(IsLoaded));
		}

		public void Initialize(Process process)
		{
			Close();

			ApplicationFilePath = process.Modules[0].FileName;
			Application = Application.Attach(process.MainWindowHandle);
			Application.Closed += Close;
			Application.Timeout = TimeSpan.FromSeconds(5);

			OnPropertyChanged(nameof(Application));
			OnPropertyChanged(nameof(IsLoaded));
		}

		private void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}

			if (Application != null)
			{
				Close();
			}
		}

		private void OnClosed()
		{
			Closed?.Invoke();
		}

		private void OnPropertyChanged(string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region Events

		public event Action Closed;
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}