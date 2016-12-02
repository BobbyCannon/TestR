#region References

using System.Drawing;
using System.IO;
using System.Linq;
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

		private static string _applicationPath;

		#endregion

		#region Methods

		[TestMethod]
		public void ApplicationLocation()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var expected = application.First<Window>("ParentForm");
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

				var expected = application.First<Window>("ParentForm");
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
				var expected = application.First<Window>("ParentForm");
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
				var window = application.First<Window>("ParentForm");
				var checkbox = window.First<CheckBox>("checkBox3");
				Assert.AreEqual(ToggleState.Indeterminate, checkbox.CheckedState);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxCheckedStateShouldBeOff()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.First<Window>("ParentForm");
				var checkbox = window.First<CheckBox>("checkBox1");
				Assert.AreEqual(ToggleState.Off, checkbox.CheckedState);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxCheckedStateShouldBeOn()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.First<Window>("ParentForm");
				var checkbox = window.First<CheckBox>("checkBox2");
				Assert.AreEqual(ToggleState.On, checkbox.CheckedState);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxCount()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.First<Window>(x => x.Id == "FormMain");
				var checkboxes = window.Descendants<CheckBox>();
				// 4 outside list, 5 inside list
				Assert.AreEqual(9, checkboxes.Count());
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxWithIndeterminateStateShouldBeChecked()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.First<Window>("ParentForm");
				var checkbox = window.First<CheckBox>("checkBox3");
				Assert.IsTrue(checkbox.Checked);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxWithOffStateShouldBeChecked()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.First<Window>("ParentForm");
				var checkbox = window.First<CheckBox>("checkBox1");
				Assert.IsFalse(checkbox.Checked);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxWithOnStateShouldBeChecked()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.First<Window>("ParentForm");
				var checkbox = window.First<CheckBox>("checkBox2");
				Assert.IsTrue(checkbox.Checked);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckWindowId()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.First<Window>("ParentForm");
				Assert.AreEqual("ParentForm", window.Id);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckWindowName()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.First<Window>("ParentForm");
				Assert.AreEqual("ParentForm", window.Name);
				application.Close();
			}
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			Application.CloseAll(_applicationPath);
		}

		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var path = Path.GetDirectoryName(assembly.Location);
			var info = new DirectoryInfo(path ?? "/");

			_applicationPath = info.Parent?.Parent?.Parent?.FullName;
			_applicationPath += "\\TestR.TestWinForms\\Bin\\" + (assembly.IsAssemblyDebugBuild() ? "Debug" : "Release") + "\\TestR.TestWinForms.exe";
		}

		[TestMethod]
		public void GetMainMenuBar()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.First<Window>("ParentForm");
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
				var window = application.First<Window>("ParentForm");
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
				var window = application.First<Window>("ParentForm");
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
				var window = application.First<Window>("ParentForm");
				checkbox = window.First<CheckBox>("checkBox1");
			}

			var element = DesktopElement.FromPoint(checkbox.Location);
			Assert.AreEqual("checkBox1", element.FullId);

			element.UpdateParents();
			Assert.AreEqual(checkbox.FullId, element.FullId);
		}

		[TestMethod]
		public void GetWindowById()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.First<Window>("FormMain");
				Assert.IsNotNull(window);
			}
		}

		[TestMethod]
		public void GetWindowByName()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.First<Window>("TestR Test WinForm");
				Assert.IsNotNull(window);
			}
		}

		[TestMethod]
		public void RefreshingApplicationWhileClosingWindowsShouldNotFail()
		{
			using (var application = Application.Create(_applicationPath))
			{
				var tempApplication = application;
				var window = application.First<Window>("TestR Test WinForm");
				Assert.IsNotNull(window);
				Task.Run(() => tempApplication.Refresh());
				window.Close();
			}
		}

		#endregion
	}
}