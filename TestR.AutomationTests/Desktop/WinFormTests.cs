#region References

using System.Drawing;
using System.Linq;
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
	public class WinFormTests : BaseTest
	{
		#region Methods

		[TestMethod]
		public void ApplicationLocation()
		{
			using (var application = GetApplication())
			{
				var expected = application.First<Window>("ParentForm");
				var wait = application.Wait(x => application.Location.X > 0);
				Assert.IsTrue(wait, "Application never displayed?");
				Assert.AreEqual(expected.Location, application.Location);
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
				var wait = application.Wait(x => application.Size.Height > 0);
				Assert.IsTrue(wait, "Application never displayed?");
				Assert.AreEqual(expected.Size, application.Size);
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
				window.Refresh();

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
		public void CheckProcessIs64Bit()
		{
			using (var application = GetApplication())
			{
				application.WaitForComplete(500);
				Assert.IsTrue(application.Process.Is64Bit, "Application should be 64 bit.");
				application.Close();
			}

			using (var application = GetApplication(true))
			{
				application.WaitForComplete(500);
				Assert.IsFalse(application.Process.Is64Bit, "Application should be 32 bit.");
				application.Close();
			}
		}

		[TestMethod]
		public void CheckWindowId()
		{
			using (var application = GetApplication())
			{
				application.WaitForComplete(500);
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
				application.WaitForComplete(500);
				var window = application.First<Window>("ParentForm");
				Assert.AreEqual("ParentForm", window.Name);
				application.Close();
			}
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
				var mainMenu = window.First("menuStrip");
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