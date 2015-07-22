#region References

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
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

		public Project()
		{
			_actions = new ObservableCollection<ElementAction>();
			_application = null;
			ApplicationFilePath = string.Empty;
			Elements = new ObservableCollection<ElementReference>();
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
		public ObservableCollection<ElementReference> Elements { get; set; }

		/// <summary>
		/// Returns true if the application is loaded.
		/// </summary>
		public bool IsLoaded => Application != null;

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
						builder.AppendLine("    application.WaitForChild<Element>(\"" + action.ApplicationId + "\").TypeText(\"" + action.Input + "\");");
						break;

					case ElementActionType.MoveMouseTo:
						builder.AppendLine("    application.WaitForChild<Element>(\"" + action.ApplicationId + "\").MoveMouseTo();");
						break;

					case ElementActionType.LeftMouseClick:
						builder.AppendLine("    application.WaitForChild<Element>(\"" + action.ApplicationId + "\").Click();");
						break;

					case ElementActionType.RightMouseClick:
						builder.AppendLine("    application.WaitForChild<Element>(\"" + action.ApplicationId + "\").RightClick();");
						break;
				}
			}

			builder.AppendLine("}");

			return builder.ToString();
		}

		public void Close()
		{
			Application?.Dispose();
			Application = null;
			OnPropertyChanged(nameof(IsLoaded));
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public Element GetElement(string applicationId)
		{
			return Application.Descendants().FirstOrDefault(x => x.ApplicationId == applicationId);
		}

		public void Initialize(string applicationPath)
		{
			Close();

			ApplicationFilePath = applicationPath;
			if (Application.Exists(ApplicationFilePath))
			{
				Application = Application.Attach(ApplicationFilePath);
				Application.Close();
			}

			Application = Application.Create(ApplicationFilePath);
			Application.Closed += Close;
			Application.Timeout = TimeSpan.FromMinutes(5);

			OnPropertyChanged(nameof(Application));
			OnPropertyChanged(nameof(IsLoaded));
		}

		public void Initialize(Project project)
		{
			Initialize(project.ApplicationFilePath);
			ElementActions.AddRange(project.ElementActions);
		}

		public void RefreshElements()
		{
			Application.UpdateChildren();
			Elements.Clear();
			Application.Children.ForEach(x => Elements.Add(CreateElementReference(x)));
		}

		public void RunAction(ElementAction action)
		{
			var element = Application.WaitForChild(action.ApplicationId);
			if (element == null)
			{
				throw new InstanceNotFoundException("Failed to find the element.");
			}

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
		}

		public void RunTests()
		{
			Initialize(ApplicationFilePath);

			foreach (var action in ElementActions)
			{
				RunAction(action);
				//Thread.Sleep(500);
			}
		}

		private ElementReference CreateElementReference(Element element)
		{
			var reference = new ElementReference(element);
			element.Children.ForEach(x => reference.Children.Add(CreateElementReference(x)));
			return reference;
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

		private void OnPropertyChanged(string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}