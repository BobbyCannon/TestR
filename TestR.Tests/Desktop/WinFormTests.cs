﻿#region References

using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop;
using TestR.Desktop.Elements;
using TestR.Desktop.Pattern;

#endregion

namespace TestR.Tests.Desktop
{
	[TestClass]
	public class WinFormTests
	{
		#region Methods

		[TestMethod]
		public void ApplicationLocation()
		{
			using var application = TestHelper.GetApplication();
			var expected = application.First<Window>("ParentForm");
			var wait = application.Wait(x => application.Location.X > 0, 1000, 20);
			Assert.IsTrue(wait, "Application never displayed?");
			Assert.AreEqual(expected.Location, application.Location);
		}

		[TestMethod]
		public void ApplicationLocationWhileMaximized()
		{
			Input.Mouse.MoveTo(0, 0);

			using var application = TestHelper.GetApplication();
			application.BringToFront();

			var expected = application.First<Window>("ParentForm");
			expected.TitleBar.MaximizeButton.Click();
			Thread.Sleep(10);

			var actual = application.Location;
			Assert.AreEqual(new Point(0, 0), actual);
		}

		[TestMethod]
		public void ApplicationSize()
		{
			using var application = TestHelper.GetApplication();
			var expected = application.First<Window>("ParentForm");
			var wait = application.Wait(x => application.Size.Height > 0);
			Assert.IsTrue(wait, "Application never displayed?");
			Assert.AreEqual(expected.Size, application.Size);
		}

		[TestMethod]
		public void CheckBoxCanBeToggledWithMouse()
		{
			using var application = TestHelper.GetApplication();
			var window = application.First<Window>("ParentForm");
			var checkbox = window.First<CheckBox>("checkBox1");
			Assert.IsFalse(checkbox.Checked);
			checkbox.LeftClick();
			Assert.IsTrue(checkbox.Checked);
			checkbox.LeftClick();
			Assert.IsFalse(checkbox.Checked);
		}

		[TestMethod]
		public void CheckBoxCheckedStateShouldBeIndeterminate()
		{
			using var application = TestHelper.GetApplication();
			var window = application.First<Window>("ParentForm");
			var checkbox = window.First<CheckBox>("checkBox3");
			Assert.AreEqual(ToggleState.Indeterminate, checkbox.CheckedState);
		}

		[TestMethod]
		public void CheckBoxCheckedStateShouldBeOff()
		{
			using var application = TestHelper.GetApplication();
			var window = application.First<Window>("ParentForm");
			var checkbox = window.First<CheckBox>("checkBox1");
			Assert.AreEqual(ToggleState.Off, checkbox.CheckedState);
		}

		[TestMethod]
		public void CheckBoxCheckedStateShouldBeOn()
		{
			using var application = TestHelper.GetApplication();
			var window = application.First<Window>("ParentForm");
			var checkbox = window.First<CheckBox>("checkBox2");
			Assert.AreEqual(ToggleState.On, checkbox.CheckedState);
		}

		[TestMethod]
		public void CheckBoxCount()
		{
			using var application = TestHelper.GetApplication();
			var window = application.First<Window>(x => x.Id == "FormMain");
			window.Refresh();

			var checkboxes = window.Descendants<CheckBox>();
			// 4 outside list, 5 inside list
			Assert.AreEqual(9, checkboxes.Count());
		}

		[TestMethod]
		public void CheckBoxWithIndeterminateStateShouldBeChecked()
		{
			using var application = TestHelper.GetApplication();
			var window = application.First<Window>("ParentForm");
			var checkbox = window.First<CheckBox>("checkBox3");
			Assert.IsTrue(checkbox.Checked);
		}

		[TestMethod]
		public void CheckBoxWithOffStateShouldBeChecked()
		{
			using var application = TestHelper.GetApplication();
			var window = application.First<Window>("ParentForm");
			var checkbox = window.First<CheckBox>("checkBox1");
			Assert.IsFalse(checkbox.Checked);
		}

		[TestMethod]
		public void CheckBoxWithOnStateShouldBeChecked()
		{
			using var application = TestHelper.GetApplication();
			var window = application.First<Window>("ParentForm");
			var checkbox = window.First<CheckBox>("checkBox2");
			Assert.IsTrue(checkbox.Checked);
		}

		[TestMethod]
		public void CheckProcessIs64Bit()
		{
			using (var application = TestHelper.GetApplication())
			{
				application.WaitForComplete(500);
				Assert.IsTrue(application.Process.Is64Bit, "Application should be 64 bit.");
				application.Close();
			}

			using (var application = TestHelper.GetApplication(true))
			{
				application.WaitForComplete(500);
				Assert.IsFalse(application.Process.Is64Bit, "Application should be 32 bit.");
				application.Close();
			}
		}

		[TestMethod]
		public void CheckWindowId()
		{
			using var application = TestHelper.GetApplication();
			application.WaitForComplete(500);
			var window = application.First<Window>("ParentForm");
			Assert.AreEqual("ParentForm", window.Id);
		}

		[TestMethod]
		public void CheckWindowName()
		{
			using var application = TestHelper.GetApplication();
			application.WaitForComplete(500);
			var window = application.First<Window>("ParentForm");
			Assert.AreEqual("ParentForm", window.Name);
		}

		[TestMethod]
		public void ClickMainMenu()
		{
			using var application = TestHelper.GetApplication();
			application.AutoClose = false;
			var window = application.First<Window>("ParentForm");
			var mainMenu = window.First("menuStrip");
			var file = mainMenu.First<MenuItem>("File").Click();
			file.First("Exit").Click(refresh: false);
		}

		[TestMethod]
		public void CloseAll()
		{
			using var application1 = Application.Create(TestHelper.ApplicationPath);
			using var application2 = Application.Create(TestHelper.ApplicationPath);
			application1.AutoClose = false;
			application2.AutoClose = false;
			Assert.IsTrue(application1.IsRunning);
			Assert.IsTrue(application2.IsRunning);
			Assert.AreNotEqual(application1.Id, application2.Id);

			Application.CloseAll(TestHelper.ApplicationPath, exceptProcessId: application2.Process.Id);

			Assert.IsFalse(application1.IsRunning);
			Assert.IsTrue(application2.IsRunning);
			Assert.AreNotEqual(application1.Id, application2.Id);
		}

		[TestMethod]
		public void GetMainMenuBar()
		{
			using var application = TestHelper.GetApplication();
			var window = application.First<Window>("ParentForm");
			var mainMenu = window.First("menuStrip");
			Assert.AreEqual("menuStrip", mainMenu.Id);
			Assert.AreEqual("MenuStrip", mainMenu.Name);
		}

		[TestMethod]
		public void GetMainStatusStrip()
		{
			using var application = TestHelper.GetApplication();
			var window = application.First<Window>("ParentForm");
			var statusBar = window.StatusBar;
			Assert.IsNotNull(statusBar);
			Assert.AreEqual("statusStrip", statusBar.Id);
			Assert.AreEqual("StatusStrip", statusBar.Name);
		}

		[TestMethod]
		public void GetMainTitleBar()
		{
			using var application = TestHelper.GetApplication();
			var window = application.First<Window>("ParentForm");
			var titleBar = window.TitleBar;
			Assert.IsNotNull(titleBar);
			Assert.AreEqual("", titleBar.Id);
			Assert.AreEqual(null, titleBar.Name);
		}

		[TestMethod]
		public void GetParents()
		{
			using var application = TestHelper.GetApplication();
			var window = application.First<Window>("ParentForm");
			var checkbox = window.First<CheckBox>("checkBox1");

			var element = DesktopElement.FromPoint(checkbox.Location);
			Assert.AreEqual("checkBox1", element.FullId);

			element.UpdateParents();
			Assert.AreEqual(checkbox.FullId, element.FullId);
		}

		[TestMethod]
		public void GetWindowById()
		{
			using var application = TestHelper.GetApplication();
			var window = application.First<Window>("FormMain");
			Assert.IsNotNull(window);
		}

		[TestMethod]
		public void GetWindowByName()
		{
			using var application = TestHelper.GetApplication();
			var window = application.First<Window>("TestR Test WinForm");
			Assert.IsNotNull(window);
		}

		[TestMethod]
		public void RefreshingApplicationWhileClosingWindowsShouldNotFail()
		{
			using var application = TestHelper.GetApplication();
			var tempApplication = application;
			var window = application.First<Window>("TestR Test WinForm");
			Assert.IsNotNull(window);
			Task.Run(() => tempApplication.Refresh());
			window.Close();
		}

		#endregion
	}
}