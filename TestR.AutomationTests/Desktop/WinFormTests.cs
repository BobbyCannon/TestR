﻿#region References

using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop;
using TestR.Desktop.Elements;
using TestR.Desktop.Pattern;
using TestR.Native;

#endregion

namespace TestR.AutomationTests.Desktop
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "WinForms")]
	public class WinFormTests : BaseTest
	{
		#region Methods

		[TestMethod]
		public void ApplicationLocation()
		{
			using (var application = GetApplication())
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
			Mouse.MoveTo(0, 0);
			using (var application = GetApplication())
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
			using (var application = GetApplication())
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
			using (var application = GetApplication())
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
			using (var application = GetApplication())
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
			using (var application = GetApplication())
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
			using (var application = GetApplication())
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
			using (var application = GetApplication())
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
			using (var application = GetApplication())
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
			using (var application = GetApplication())
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
			using (var application = GetApplication())
			{
				var window = application.First<Window>("ParentForm");
				Assert.AreEqual("ParentForm", window.Id);
				application.Close();
			}
		}

		[TestMethod]
		public void CheckWindowName()
		{
			using (var application = GetApplication())
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

		[TestMethod]
		public void CloseAll()
		{
			using (var application1 = Application.Create(_applicationPath))
			{
				using (var application2 = Application.Create(_applicationPath))
				{
					Assert.IsTrue(application1.IsRunning);
					Assert.IsTrue(application2.IsRunning);
					Assert.AreNotEqual(application1.Id, application2.Id);

					Application.CloseAll(_applicationPath, exceptProcessId: application2.Process.Id);

					Assert.IsFalse(application1.IsRunning);
					Assert.IsTrue(application2.IsRunning);
					Assert.AreNotEqual(application1.Id, application2.Id);
				}
			}
		}

		[TestMethod]
		public void GetMainMenuBar()
		{
			using (var application = GetApplication())
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
			using (var application = GetApplication())
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
			using (var application = GetApplication())
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
			using (var application = GetApplication())
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
			using (var application = GetApplication())
			{
				var window = application.First<Window>("FormMain");
				Assert.IsNotNull(window);
			}
		}

		[TestMethod]
		public void GetWindowByName()
		{
			using (var application = GetApplication())
			{
				var window = application.First<Window>("TestR Test WinForm");
				Assert.IsNotNull(window);
			}
		}

		[TestMethod]
		public void RefreshingApplicationWhileClosingWindowsShouldNotFail()
		{
			using (var application = GetApplication())
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