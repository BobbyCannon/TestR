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
		private string _applicationFilePath;

		#endregion

		#region Constructors

		public Project(string applicationFilePath)
		{
			_actions = new ObservableCollection<ElementAction>();
			ApplicationFilePath = applicationFilePath;
			Elements = new ObservableCollection<ElementReference>();
		}

		#endregion

		#region Properties

		[JsonIgnore]
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

		[JsonIgnore]
		public ObservableCollection<ElementReference> Elements { get; set; }

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
						builder.AppendLine("    application.GetChild<Element>(\"" + action.ApplicationId + "\").TypeText(\"" + action.Input + "\");");
						break;

					case ElementActionType.LeftMouseClick:
						builder.AppendLine("    application.GetChild<Element>(\"" + action.ApplicationId + "\").Click();");
						break;

					case ElementActionType.RightMouseClick:
						builder.AppendLine("    application.GetChild<Element>(\"" + action.ApplicationId + "\").RightClick();");
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

		public Element GetElement(string applicationId)
		{
			return Application.Descendants().FirstOrDefault(x => x.ApplicationId == applicationId);
		}

		public void Initialize()
		{
			if (Application != null)
			{
				Application.Dispose();
				Application = null;
			}

			if (Application.Exists(ApplicationFilePath))
			{
				Application = Application.Attach(ApplicationFilePath);
				Application.Close();
			}

			Application = Application.Create(ApplicationFilePath);
		}

		public void RefreshElements()
		{
			Application.UpdateChildren();
			Elements.Clear();
			Application.Children.ForEach(x => Elements.Add(CreateElementReference(x)));
		}

		public void RunTests()
		{
			Initialize();

			foreach (var action in ElementActions)
			{
				var element = Application.Children.GetChild(action.ApplicationId);
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

				Thread.Sleep(500);
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