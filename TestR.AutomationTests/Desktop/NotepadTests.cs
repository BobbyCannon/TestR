#region References

using System;
using System.Linq;
using System.Management.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop.Elements;
using TestR.PowerShell;

#endregion

namespace TestR.AutomationTests.Desktop
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "Notepad")]
	public class NotepadTests : TestCmdlet
	{
		#region Fields

		private static readonly string _applicationPath = "C:\\Windows\\Notepad.exe";

		#endregion

		#region Methods

		[TestMethod]
		public void AddTextToDocument()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.Children.First();
				var document = (Edit) window.Children.First("15");
				document.SetText("Hello World : Sub Collection");
			}
		}

		[TestMethod]
		public void AddTextToDocumentUsingGetWithGeneric()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var document = application.First<Edit>("15");
				document.Text = "Hello World : GetChild Generic";
			}
		}

		[TestMethod]
		public void AddTextToDocumentUsingGetWithType()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.First<Window>();
				var document = (Edit) window.First("15");
				document.Text = "Hello World : GetChild Non Generic";
			}
		}

		[TestMethod]
		public void ApplicationAttachShouldSucceed()
		{
			Application.CloseAll(_applicationPath);

			using (var application1 = Application.Create(_applicationPath))
			{
				Assert.IsNotNull(application1);

				using (var application2 = Application.Attach(_applicationPath))
				{
					Assert.IsNotNull(application2);
					Assert.AreEqual(application1.Handle, application2.Handle);
				}
			}
		}

		[TestMethod]
		public void ApplicationBringToFrontShouldSucceed()
		{
			using (var application2 = Application.AttachOrCreate(_applicationPath))
			{
				Assert.IsNotNull(application2);
				application2.BringToFront();
				Assert.IsTrue(application2.IsInFront());
			}
		}

		[TestMethod]
		public void ApplicationCreateShouldSucceed()
		{
			Application.CloseAll(_applicationPath);

			using (var application = Application.Create(_applicationPath))
			{
				Assert.IsNotNull(application);
			}
		}

		[TestMethod]
		public void ApplicationListElements()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				foreach (var window in application.Descendants<Window>())
				{
					window.Refresh();
				}
			}
		}

		[TestMethod]
		public void ClickMenu()
		{
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				application.BringToFront();
				var window = application.Descendants<Window>().First();
				var menuBar = window.Descendants<MenuBar>().First();
				TestHelper.PrintChildren(menuBar);

				var menu = menuBar.First<MenuItem>(x => x.Name == "File");
				Assert.IsNotNull(menu);
				Assert.IsTrue(menu.SupportsExpandingCollapsing);
				menu.Click();
			}
		}

		[TestMethod]
		public void Screenshot()
		{
			var filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Test.png";
			Application.CloseAll(_applicationPath);
			using (var application = Application.AttachOrCreate(_applicationPath))
			{
				var window = application.First<Window>(x => x.Name == "Untitled - Notepad");
				window.TitleBar.CaptureSnippet(filePath);
			}
		}

		[TestMethod]
		public void WaitForButtons()
		{
			Application.CloseAll(_applicationPath);
			using (var application = Application.AttachOrCreate(_applicationPath))
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