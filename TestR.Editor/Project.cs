#region References

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading;
using TestR.Desktop;
using TestR.Native;

#endregion

namespace TestR.Editor
{
	public sealed class Project : IDisposable, INotifyPropertyChanged
	{
		#region Fields

		private ObservableCollection<ElementAction> _actions;
		private string _applicationFilePath;

		#endregion

		#region Constructors

		public Project(string applicationFilePath)
		{
			_actions = new ObservableCollection<ElementAction>();
			Application = Application.AttachOrCreate(applicationFilePath);
			ApplicationFilePath = applicationFilePath;
		}

		#endregion

		#region Properties

		public Application Application { get; set; }

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
			get { return Application.Children; }
		}

		#endregion

		#region Methods

		public string Build()
		{
			var builder = new StringBuilder();

			builder.AppendLine("using (var application = Application.AttachOrCreate(@\"" + ApplicationFilePath + "\"))");
			builder.AppendLine("{");

			foreach (var action in ElementActions)
			{
				switch (action.Type)
				{
					case ElementActionType.TypeText:
						builder.AppendLine("    application.GetChild<Element>(\"" + action.ElementId + "\").TypeText(\"" + action.Input + "\");");
						break;
					
					case ElementActionType.LeftMouseClick:
						builder.AppendLine("    application.GetChild<Element>(\"" + action.ElementId + "\").Click();");
						break;
					
					case ElementActionType.RightMouseClick:
						builder.AppendLine("    application.GetChild<Element>(\"" + action.ElementId + "\").RightClick();");
						break;
				}
			}

			builder.AppendLine("}");

			return builder.ToString();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void RefreshElements()
		{
			Application.UpdateChildren();
			Application.BringToFront();
		}

		public void RunTests()
		{
			foreach (var action in ElementActions)
			{
				var element = Application.Children.GetChild(action.ElementId);

				switch (action.Type)
				{
					case ElementActionType.MoveMouseTo:
						element.MoveMouseTo();
						break;

					case ElementActionType.LeftMouseClick:
						element.Click();
						break;

					case ElementActionType.RightMouseClick:
						element.RightClick();
						break;

					case ElementActionType.TypeText:
						element.TypeText(action.Input);
						break;
				}

				Thread.Sleep(500);
			}
		}

		private void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}

			if (Application != null)
			{
				Application.Dispose();
				Application = null;
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