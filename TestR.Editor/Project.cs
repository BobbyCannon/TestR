#region References

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TestR.Desktop;
using TestR.Extensions;

#endregion

namespace TestR.Editor
{
	public sealed class Project : IDisposable, INotifyPropertyChanged
	{
		#region Fields

		private ObservableCollection<ElementAction> _actions;
		private Application _application;
		private string _applicationFilePath;

		#endregion

		#region Constructors

		public Project(string applicationFilePath)
		{
			_actions = new ObservableCollection<ElementAction>();
			_application = Application.AttachOrCreate(applicationFilePath);

			ApplicationFilePath = applicationFilePath;
		}

		#endregion

		#region Properties

		public string ApplicationFilePath
		{
			get { return _applicationFilePath; }
			set
			{
				_applicationFilePath = value;
				OnPropertyChanged("ApplicationFilePath");
			}
		}

		public ObservableCollection<ElementAction> ElementActions
		{
			get { return _actions; }
			set
			{
				_actions = value;
				OnPropertyChanged("ElementActions");
			}
		}

		public ElementCollection<Element> Elements
		{
			get { return _application.Children; }
		}

		#endregion

		#region Methods

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void RefreshElements()
		{
			_application.UpdateChildren();
			_application.BringToFront();
		}

		public void RunTests()
		{
		}

		private void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}

			if (_application != null)
			{
				_application.Dispose();
				_application = null;
			}
		}

		private void OnPropertyChanged(string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}