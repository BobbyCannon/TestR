#region References

using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop;
using TestR.Desktop.Elements;
using TestR.Desktop.Pattern;
using TestR.Extensions;
using TestR.PowerShell;

#endregion

namespace TestR.IntegrationTests.Desktop
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "WinForms")]
	public class WinFormTests : TestCmdlet
	{
		#region Fields

		public static string ApplicationPath;

		#endregion

		#region Methods

		[TestMethod]
		public void CheckBoxCheckedStateShouldBeIndeterminate()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Children.Windows.First();
				var checkbox = window.WaitForChild<CheckBox>("checkBox3");
				Assert.AreEqual(ToggleState.Indeterminate, checkbox.CheckedState);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxCheckedStateShouldBeOff()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Children.Windows.First();
				var checkbox = window.WaitForChild<CheckBox>("checkBox1");
				Assert.AreEqual(ToggleState.Off, checkbox.CheckedState);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxCheckedStateShouldBeOn()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Children.Windows.First();
				var checkbox = window.WaitForChild<CheckBox>("checkBox2");
				Assert.AreEqual(ToggleState.On, checkbox.CheckedState);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxCount()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Children.Windows.First();
				var checkbox = window.Children.CheckBoxes;
				Assert.AreEqual(4, checkbox.Count);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxWithIndeterminateStateShouldBeChecked()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Children.Windows.First();
				var checkbox = window.WaitForChild<CheckBox>("checkBox3");
				Assert.IsTrue(checkbox.Checked);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxWithOffStateShouldBeChecked()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Children.Windows.First();
				var checkbox = window.WaitForChild<CheckBox>("checkBox1");
				Assert.IsFalse(checkbox.Checked);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckBoxWithOnStateShouldBeChecked()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Children.Windows.First();
				var checkbox = window.WaitForChild<CheckBox>("checkBox2");
				Assert.IsTrue(checkbox.Checked);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckWindowId()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Children.Windows.First();
				Assert.AreEqual("FormMain", window.Id);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckWindowName()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Children.Windows.First();
				Assert.AreEqual("TestR Test WinForm", window.Name);
				application.Close();
			}
		}

		[TestMethod]
		public void GetMainMenuBar()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Children.Windows.First();
				TestHelper.PrintChildren(window);
				var mainMenu = window.Children["MenuStrip"];
				Assert.AreEqual("MenuStrip", mainMenu.Id);
				Assert.AreEqual("mainMenuStrip", mainMenu.Name);
				application.Close();
			}
		}

		[TestMethod]
		public void GetMainStatusStrip()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Children.Windows.First();
				var statusBar = window.StatusBar;
				Assert.IsNotNull(statusBar);
				Assert.AreEqual("StatusStrip", statusBar.Id);
				Assert.AreEqual("statusStrip1", statusBar.Name);
				Assert.AreEqual("statusStrip1", statusBar.Text);
				application.Close();
			}
		}

		[TestMethod]
		public void GetMainTitleBar()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Children.Windows.First();
				var titleBar = window.TitleBar;
				Assert.IsNotNull(titleBar);
				Assert.AreEqual("", titleBar.Id);
				Assert.AreEqual(null, titleBar.Name);
				Assert.AreEqual(null, titleBar.Text);
				application.Close();
			}
		}

		[TestMethod]
		public void GetWindowById()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Children.Windows["FormMain"];
				Assert.IsNotNull(window);
			}
		}

		[TestMethod]
		public void GetWindowByName()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Children.FirstOrDefault(x => x.Name == "TestR Test WinForm");
				Assert.IsNotNull(window);
			}
		}

		[TestInitialize]
		public void Setup()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var path = Path.GetDirectoryName(assembly.Location);
			var info = new DirectoryInfo(path ?? "/");

			ApplicationPath = info.Parent?.Parent?.Parent?.FullName;
			ApplicationPath += "\\TestR.TestWinForms\\Bin\\" + (assembly.IsAssemblyDebugBuild() ? "Debug" : "Release") + "\\TestR.TestWinForms.exe";
		}

		#endregion
	}
}