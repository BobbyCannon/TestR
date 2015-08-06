#region References

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
						if (action.Input.Contains(","))
						{
							var points = action.Input.Split(",");
							if (points.Length >= 2)
							{
								builder.AppendLine("    application.WaitForChild<Element>(\"" + action.ApplicationId + "\").MoveMouseTo(" + int.Parse(points[0]) + "," + int.Parse(points[1]) + ");");
								break;
							}
						}
						builder.AppendLine("    application.WaitForChild<Element>(\"" + action.ApplicationId + "\").MoveMouseTo();");
						break;

					case ElementActionType.LeftMouseClick:
						if (action.Input.Contains(","))
						{
							var points = action.Input.Split(",");
							if (points.Length >= 2)
							{
								builder.AppendLine("    application.WaitForChild<Element>(\"" + action.ApplicationId + "\").Click(" + int.Parse(points[0]) + "," + int.Parse(points[1]) + ");");
								break;
							}
						}
						builder.AppendLine("    application.WaitForChild<Element>(\"" + action.ApplicationId + "\").Click();");
						break;

					case ElementActionType.RightMouseClick:
						if (action.Input.Contains(","))
						{
							var points = action.Input.Split(",");
							if (points.Length >= 2)
							{
								builder.AppendLine("    application.WaitForChild<Element>(\"" + action.ApplicationId + "\").RightClick(" + int.Parse(points[0]) + "," + int.Parse(points[1]) + ");");
								break;
							}
						}
						builder.AppendLine($"    application.WaitForChild<Element>(\"{action.ApplicationId}\").RightClick();");
						break;

					case ElementActionType.Equals:
						builder.AppendLine($"    Assert.AreEqual(\"{action.Input}\", application.WaitForChild<Element>(\"{action.ApplicationId}\").{action.Property}.ToString());");
						break;

					case ElementActionType.NotEqual:
						builder.AppendLine($"    Assert.AreNotEqual(\"{action.Input}\", application.WaitForChild<Element>(\"{action.ApplicationId}\").{action.Property}.ToString());");
						break;

					case ElementActionType.Exists:
						builder.AppendLine($"    Assert.IsNotNull(application.GetChild<Element>(\"{action.ApplicationId}\"));");
						break;

					case ElementActionType.NotExist:
						builder.AppendLine($"    Assert.IsNull(application.GetChild<Element>(\"{action.ApplicationId}\"));");
						break;

					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			builder.AppendLine("}");

			return builder.ToString();
		}

		public void Close()
		{
			if (Application != null)
			{
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

		public Element GetElement(string applicationId)
		{
			return Application?.GetChild<Element>(x => x.ApplicationId == applicationId);
		}

		public void Initialize(string applicationPath)
		{
			Close();

			ApplicationFilePath = applicationPath;
			Application = Application.AttachOrCreate(ApplicationFilePath);
			Application.Closed += Close;
			Application.Timeout = TimeSpan.FromMinutes(5);

			OnPropertyChanged(nameof(Application));
			OnPropertyChanged(nameof(IsLoaded));
		}

		public void Initialize(Project project)
		{
			Initialize(project.ApplicationFilePath);
			ElementActions.Clear();
			ElementActions.AddRange(project.ElementActions);
		}

		public void Initialize(Process process)
		{
			Close();

			ApplicationFilePath = process.Modules[0].FileName;
			Application = Application.Attach(process.MainWindowHandle);
			Application.Closed += Close;
			Application.Timeout = TimeSpan.FromSeconds(30);

			OnPropertyChanged(nameof(Application));
			OnPropertyChanged(nameof(IsLoaded));
		}

		public void RefreshElements()
		{
			Application.UpdateChildren();
			Elements.Clear();
			Application.Children.ForEach(x => Elements.Add(CreateElementReference(x)));
		}

		public void RunAction(ElementAction action)
		{
			if (action == null)
			{
				return;
			}

			var element = Application.GetChild<Element>(action.ApplicationId);

			switch (action.Type)
			{
				case ElementActionType.TypeText:
					element.TypeText(action.Input);
					break;

				case ElementActionType.MoveMouseTo:
					element.MoveMouseTo();
					break;

				case ElementActionType.LeftMouseClick:
					if (action.Input.Contains(","))
					{
						var points = action.Input.Split(",");
						if (points.Length >= 2)
						{
							element.Click(int.Parse(points[0]), int.Parse(points[1]));
							return;
						}
					}
					element.Click();
					break;

				case ElementActionType.RightMouseClick:
					if (action.Input.Contains(","))
					{
						var points = action.Input.Split(",");
						if (points.Length >= 2)
						{
							element.RightClick(int.Parse(points[0]), int.Parse(points[1]));
							break;
						}
					}
					element.RightClick();
					break;

				case ElementActionType.Equals:
					var type1 = element.GetType();
					var property1 = type1.GetProperties().FirstOrDefault(x => x.Name == action.Property);
					Assert.AreEqual(action.Input, property1?.GetValue(element).ToString());
					break;

				case ElementActionType.NotEqual:
					var type2 = element.GetType();
					var property2 = type2.GetProperties().FirstOrDefault(x => x.Name == action.Property);
					Assert.AreNotEqual(action.Input, property2?.GetValue(element).ToString());
					break;

				case ElementActionType.Exists:
					Assert.IsNotNull(element, "The element does not exist but should have.");
					break;

				case ElementActionType.NotExist:
					Assert.IsNull(element, "The element does exist but should not have.");
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void RunTests()
		{
			Initialize(ApplicationFilePath);
			ElementActions.ForEach(RunAction);
		}

		private static ElementReference CreateElementReference(Element element)
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