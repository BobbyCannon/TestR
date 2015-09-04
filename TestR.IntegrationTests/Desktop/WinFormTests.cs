#region References

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.Remoting;
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

		private static string _applicationPath;

		#endregion

		#region Methods

		[TestMethod]
		public void ApplicationLocation()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var expected = application.Children.Windows.First();
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
				var expected = application.Children.Windows.First();
				expected.TitleBar.MaximizeButton.Click();
				expected.Location.Dump();

				var actual = application.Location;
				Assert.AreEqual(new Point(0,0), actual);
				application.Close();
			}
		}

		[TestMethod]
		public void ApplicationSize()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var expected = application.Children.Windows.First();
				var actual = application.Size;
				Assert.AreEqual(expected.Size, actual);
				application.Close();
			}
		}

		[TestMethod]
		public void GetParents()
		{
			CheckBox checkbox;
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.Windows.First();
				checkbox = window.Get<CheckBox>("checkBox1");
			}

			var element = Element.FromPoint(checkbox.Location);
			Assert.AreEqual("checkBox1", element.ApplicationId);

			element.UpdateParents();
			Assert.AreEqual(checkbox.ApplicationId, element.ApplicationId);

		}

		[TestMethod]
		public void CheckBoxCheckedStateShouldBeIndeterminate()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.Windows.First();
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
				var window = application.Children.Windows.First();
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
				var window = application.Children.Windows.First();
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
				var window = application.Children.Windows.First();
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
				var window = application.Children.Windows.First();
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
				var window = application.Children.Windows.First();
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
				var window = application.Children.Windows.First();
				Assert.AreEqual("ParentForm", window.Id);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckWindowName()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.Windows.First();
				Assert.AreEqual("ParentForm", window.Name);
				application.Close();
			}
		}

		[TestMethod]
		public void GetMainMenuBar()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.Windows.First();
				TestHelper.PrintChildren(window);
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
				var window = application.Children.Windows.First();
				var statusBar = window.StatusBar;
				Assert.IsNotNull(statusBar);
				Assert.AreEqual("statusStrip", statusBar.Id);
				Assert.AreEqual("StatusStrip", statusBar.Name);
				Assert.AreEqual("StatusStrip", statusBar.Text);
				application.Close();
			}
		}

		[TestMethod]
		public void GetMainTitleBar()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
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
				var window = application.Get<Window>(x => x.Name == "TestR Test WinForm");
				Assert.IsNotNull(window);
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