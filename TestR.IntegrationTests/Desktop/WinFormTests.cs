#region References

using System.Drawing;
using System.IO;
using System.Management.Automation;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop;
using TestR.Desktop.Elements;
using TestR.Desktop.Pattern;
using TestR.PowerShell;

#endregion

namespace TestR.AutomationTests.Desktop
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "WinForms")]
	public class WinFormTests : TestCmdlet
	{
		#region Fields

		private string _applicationPath;

		#endregion

		#region Methods

		[TestMethod]
		public void ApplicationLocation()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var expected = application.Children.Windows["ParentForm"];
				var actual = application.Location;
				Assert.AreEqual(expected.Location, actual);
				application.Close();
			}
		}

		[TestMethod]
		public void ApplicationLocationWhileMaximized()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				application.BringToFront();

				var expected = application.Children.Windows["ParentForm"];
				expected.TitleBar.MaximizeButton.Click();

				var actual = application.Location;
				Assert.AreEqual(new Point(0, 0), actual);
				application.Close();
			}
		}

		[TestMethod]
		public void ApplicationSize()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var expected = application.Children.Windows["ParentForm"];
				var actual = application.Size;
				Assert.AreEqual(expected.Size, actual);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxCheckedStateShouldBeIndeterminate()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.Windows["ParentForm"];
				var checkbox = window.Get<CheckBox>("checkBox3");
				Assert.AreEqual(ToggleState.Indeterminate, checkbox.CheckedState);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxCheckedStateShouldBeOff()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.Windows["ParentForm"];
				var checkbox = window.Get<CheckBox>("checkBox1");
				Assert.AreEqual(ToggleState.Off, checkbox.CheckedState);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxCheckedStateShouldBeOn()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.Windows["ParentForm"];
				var checkbox = window.Get<CheckBox>("checkBox2");
				Assert.AreEqual(ToggleState.On, checkbox.CheckedState);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxCount()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Get<Window>(x => x.Id == "FormMain");
				var checkbox = window.Children.CheckBoxes;
				Assert.AreEqual(4, checkbox.Count);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxWithIndeterminateStateShouldBeChecked()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.Windows["ParentForm"];
				var checkbox = window.Get<CheckBox>("checkBox3");
				Assert.IsTrue(checkbox.Checked);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxWithOffStateShouldBeChecked()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.Windows["ParentForm"];
				var checkbox = window.Get<CheckBox>("checkBox1");
				Assert.IsFalse(checkbox.Checked);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxWithOnStateShouldBeChecked()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.Windows["ParentForm"];
				var checkbox = window.Get<CheckBox>("checkBox2");
				Assert.IsTrue(checkbox.Checked);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckWindowId()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.Windows["ParentForm"];
				Assert.AreEqual("ParentForm", window.Id);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckWindowName()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.Windows["ParentForm"];
				Assert.AreEqual("ParentForm", window.Name);
				application.Close();
			}
		}

		[TestMethod]
		public void GetMainMenuBar()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.Windows["ParentForm"];
				var mainMenu = window.Children["menuStrip"];
				Assert.AreEqual("menuStrip", mainMenu.Id);
				Assert.AreEqual("MenuStrip", mainMenu.Name);
				application.Close();
			}
		}

		[TestMethod]
		public void GetMainStatusStrip()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.Windows["ParentForm"];
				var statusBar = window.StatusBar;
				Assert.IsNotNull(statusBar);
				Assert.AreEqual("statusStrip", statusBar.Id);
				Assert.AreEqual("StatusStrip", statusBar.Name);
				application.Close();
			}
		}

		[TestMethod]
		public void GetMainTitleBar()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.Windows["ParentForm"];
				var titleBar = window.TitleBar;
				Assert.IsNotNull(titleBar);
				Assert.AreEqual("", titleBar.Id);
				Assert.AreEqual(null, titleBar.Name);
				application.Close();
			}
		}

		[TestMethod]
		public void GetParents()
		{
			CheckBox checkbox;
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.Windows["ParentForm"];
				checkbox = window.Get<CheckBox>("checkBox1");
			}

			var element = DesktopElement.FromPoint(checkbox.Location);
			Assert.AreEqual("checkBox1", element.ApplicationId);

			element.UpdateParents();
			Assert.AreEqual(checkbox.ApplicationId, element.ApplicationId);
		}

		[TestMethod]
		public void GetWindowById()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Get<Window>("FormMain");
				Assert.IsNotNull(window);
			}
		}

		[TestMethod]
		public void GetWindowByName()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Get<Window>("TestR Test WinForm");
				Assert.IsNotNull(window);
			}
		}

		[TestMethod]
		public void RefreshingApplicationWhileClosingWindowsShouldNotFail()
		{
			using (var application = Application.Create(_applicationPath))
			{
				var tempApplication = application;
				var window = application.Get<Window>("TestR Test WinForm");
				Assert.IsNotNull(window);
				Task.Run(() => tempApplication.Refresh());
				window.Close();
			}
		}

		[TestInitialize]
		public void Setup()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var path = Path.GetDirectoryName(assembly.Location);
			var info = new DirectoryInfo(path ?? "/");

			_applicationPath = info.Parent?.Parent?.Parent?.FullName;
			_applicationPath += "\\TestR.TestWinForms\\Bin\\" + (assembly.IsAssemblyDebugBuild() ? "Debug" : "Release") + "\\TestR.TestWinForms.exe";
		}

		#endregion
	}
}