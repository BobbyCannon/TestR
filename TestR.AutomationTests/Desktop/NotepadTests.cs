#region References

using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop.Elements;
using TestR.Native;
using TestR.PowerShell;

#endregion

namespace TestR.AutomationTests.Desktop
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "Notepad")]
	public class NotepadTests : TestCmdlet
	{
		#region Constants

		private const string ApplicationPath = "C:\\Windows\\Notepad.exe";

		#endregion

		#region Methods

		[TestMethod]
		public void AddTextToDocument()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				application.AutoClose = true;
				var window = application.Children.First();
				var document = (Edit) window.Children.First("15");
				var expected = "Hello World : Sub Collection";
				document.SetText(expected);
				var actual = document.Text;
				application.Timeout = TimeSpan.Zero;
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod]
		public void AddTextToDocumentUsingGetWithGeneric()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				application.AutoClose = true;
				var document = application.First<Edit>("15");
				var expected = "Hello World : GetChild Generic";
				document.Text = expected;
				var actual = document.Text;
				application.Timeout = TimeSpan.Zero;
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod]
		public void AddTextToDocumentUsingGetWithType()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				application.AutoClose = true;
				var window = application.First<Window>();
				var document = (Edit) window.First("15");
				var expected = "Hello World : GetChild Non Generic";
				document.Text = expected;
				var actual = document.Text;
				application.Timeout = TimeSpan.Zero;
				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod]
		public void ApplicationAttachShouldNotChangeMaximize()
		{
			Application.CloseAll(ApplicationPath);

			using (var application1 = Application.Create(ApplicationPath))
			{
				Assert.IsNotNull(application1);
				application1.Resize(500, 500);
				application1.MoveWindow(100, 100);

				var window = application1.First<Window>();
				Assert.IsFalse(window.IsMaximized);
				window.TitleBar.MaximizeButton.Click();
				Assert.IsTrue(window.IsMaximized);

				using (var application2 = Application.Attach(ApplicationPath))
				{
					Assert.IsNotNull(application2);
					Assert.AreEqual(application1.Handle, application2.Handle);
					Assert.IsTrue(application2.First<Window>().IsMaximized);
				}
			}
		}

		[TestMethod]
		public void ApplicationAttachShouldNotChangeMinimize()
		{
			Application.CloseAll(ApplicationPath);

			using (var application1 = Application.Create(ApplicationPath))
			{
				application1.AutoClose = true;
				Assert.IsNotNull(application1);
				application1.Resize(500, 500);
				application1.MoveWindow(100, 100);
				application1.Timeout = TimeSpan.Zero;
				
				var window = application1.First<Window>();
				Assert.IsFalse(window.IsMinimized);
				window.TitleBar.MinimizeButton.Click();
				Assert.IsTrue(window.IsMinimized);

				using (var application2 = Application.Attach(ApplicationPath))
				{
					application2.AutoClose = true;
					application2.Timeout = TimeSpan.Zero;
					Assert.IsNotNull(application2);
					Assert.AreEqual(application1.Handle, application2.Handle);
					Assert.IsTrue(application2.First<Window>().IsMinimized);
				}
			}
		}

		[TestMethod]
		public void ApplicationAttachShouldSucceed()
		{
			Application.CloseAll(ApplicationPath);

			using (var application1 = Application.Create(ApplicationPath))
			{
				Assert.IsNotNull(application1);

				using (var application2 = Application.Attach(ApplicationPath))
				{
					Assert.IsNotNull(application2);
					Assert.AreEqual(application1.Handle, application2.Handle);
				}
			}
		}

		[TestMethod]
		public void ApplicationBringToFrontShouldSucceed()
		{
			using (var application2 = Application.AttachOrCreate(ApplicationPath))
			{
				Assert.IsNotNull(application2);
				application2.BringToFront();
				Assert.IsTrue(application2.IsInFront());
			}
		}

		[TestMethod]
		public void ApplicationCreateShouldSucceed()
		{
			Application.CloseAll(ApplicationPath);

			using (var application = Application.Create(ApplicationPath))
			{
				Assert.IsNotNull(application);
			}
		}

		[TestMethod]
		public void ApplicationAutoCloseShouldSucceed()
		{
			Application.CloseAll(ApplicationPath);

			var processes = ProcessService.Where(ApplicationPath).ToList();
			processes.ForEach(x => x.Dispose());
			Assert.AreEqual(0, processes.Count);
			
			using (var application = Application.Create(ApplicationPath))
			{
				application.AutoClose = true;
				Assert.IsNotNull(application);
			}

			processes = ProcessService.Where(ApplicationPath).ToList();
			processes.ForEach(x => x.Dispose());
			Assert.AreEqual(0, processes.Count);
		}

		[TestMethod]
		public void ApplicationListElements()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				foreach (var window in application.Descendants<Window>())
				{
					window.Refresh();
				}
			}
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			Application.CloseAll(ApplicationPath);
		}

		[TestMethod]
		public void ClickMenu()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				application.AutoClose = true;
				application.BringToFront();
				var window = application.Descendants<Window>().First();
				var menuBar = window.Descendants<MenuBar>().First();
				var menu = menuBar.First<MenuItem>(x => x.Name == "File");
				Assert.IsNotNull(menu);
				Assert.IsTrue(menu.SupportsExpandingCollapsing);
				menu.Click();
				application.Timeout = TimeSpan.Zero;
			}
		}

		[TestMethod]
		public void Screenshot()
		{
			var filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Test.png";
			Application.CloseAll(ApplicationPath);
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				application.AutoClose = true;
				var window = application.First<Window>(x => x.Name == "Untitled - Notepad");
				window.TitleBar.CaptureSnippet(filePath);
				application.Timeout = TimeSpan.Zero;
			}
		}

		[TestMethod]
		public void WaitForButtons()
		{
			Application.CloseAll(ApplicationPath);
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var bar = application.First("NonClientVerticalScrollBar");
				var button = bar.First<Button>(x => x.Id == "UpButton");
				button.MoveMouseTo();
				button = bar.First<Button>(x => x.Id == "DownButton");
				button.MoveMouseTo();
			}
		}

		#endregion
	}
}