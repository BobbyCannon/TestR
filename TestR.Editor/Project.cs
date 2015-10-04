#region References

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TestR.Desktop;
using TestR.Extensions;
using Application = TestR.Desktop.Application;

#endregion

namespace TestR.Editor
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

		public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

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
						builder.AppendLine("    application.Get<Element>(\"" + action.ApplicationId + "\").TypeText(\"" + action.Input + "\");");
						break;

					case ElementActionType.MoveMouseTo:
						if (action.Input.Contains(","))
						{
							var points = action.Input.Split(",");
							if (points.Length >= 2)
							{
								builder.AppendLine("    application.Get<Element>(\"" + action.ApplicationId + "\").MoveMouseTo(" + int.Parse(points[0]) + "," + int.Parse(points[1]) + ");");
								break;
							}
						}
						builder.AppendLine("    application.Get<Element>(\"" + action.ApplicationId + "\").MoveMouseTo();");
						break;

					case ElementActionType.LeftMouseClick:
						if (action.Input.Contains(","))
						{
							var points = action.Input.Split(",");
							if (points.Length >= 2)
							{
								builder.AppendLine("    application.Get<Element>(\"" + action.ApplicationId + "\").Click(" + int.Parse(points[0]) + "," + int.Parse(points[1]) + ");");
								break;
							}
						}
						builder.AppendLine("    application.Get<Element>(\"" + action.ApplicationId + "\").Click();");
						break;

					case ElementActionType.RightMouseClick:
						if (action.Input.Contains(","))
						{
							var points = action.Input.Split(",");
							if (points.Length >= 2)
							{
								builder.AppendLine("    application.Get<Element>(\"" + action.ApplicationId + "\").RightClick(" + int.Parse(points[0]) + "," + int.Parse(points[1]) + ");");
								break;
							}
						}
						builder.AppendLine($"    application.Get<Element>(\"{action.ApplicationId}\").RightClick();");
						break;

					case ElementActionType.Equals:
						builder.AppendLine($"    Assert.AreEqual(\"{action.Input}\", application.Get<Element>(\"{action.ApplicationId}\").{action.Property}.ToString());");
						break;

					case ElementActionType.NotEqual:
						builder.AppendLine($"    Assert.AreNotEqual(\"{action.Input}\", application.Get<Element>(\"{action.ApplicationId}\").{action.Property}.ToString());");
						break;

					case ElementActionType.Exists:
						builder.AppendLine($"    Assert.IsNotNull(application.Get<Element>(\"{action.ApplicationId}\"));");
						break;

					case ElementActionType.NotExist:
						builder.AppendLine($"    Assert.IsNull(application.Get<Element>(\"{action.ApplicationId}\"));");
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

		public Element GetElement(string applicationId)
		{
			return Application?.Get<Element>(applicationId);
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
			Application.Timeout = TimeSpan.FromSeconds(5);

			OnPropertyChanged(nameof(Application));
			OnPropertyChanged(nameof(IsLoaded));
		}

		public void RunAction(ElementAction action)
		{
			if (action == null)
			{
				return;
			}

			var element = Application.Get(action.ApplicationId);
			if (element == null)
			{
				MessageBox.Show("Failed to find the element...");
				return;
			}

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
			ElementActions.ForEach(RunAction);
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