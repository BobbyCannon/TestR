#region References

using System;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.PowerShell;
using TestR.Web;
using TestR.Web.Browsers;

#endregion

namespace TestR.AutomationTests.Desktop
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "InternetExplorer")]
	public class InternetExplorerTests : TestCmdlet
	{
		#region Methods

		[TestMethod]
		public void AttachOneBrowser()
		{
			using (var browser1 = InternetExplorer.Create())
			{
				Assert.IsNotNull(browser1);
			}

			using (var browser2 = InternetExplorer.Attach())
			{
				Assert.IsNotNull(browser2);
			}
		}

		[TestMethod]
		public void AttachOrCreate()
		{
			using (var browser = InternetExplorer.AttachOrCreate())
			{
				Assert.IsNotNull(browser);
			}
		}

		[TestMethod]
		public void AttachOrCreateOneBrowser()
		{
			using (var browser = InternetExplorer.Create())
			{
				Assert.IsNotNull(browser);

				using (var browser2 = InternetExplorer.AttachOrCreate())
				{
					Assert.IsNotNull(browser2);
					Assert.AreEqual(browser.Id, browser2.Id);
				}
			}
		}

		[TestMethod]
		public void AttachToBrowser()
		{
			int processId;

			using (var browser1 = InternetExplorer.Create())
			{
				Assert.IsNotNull(browser1);
				processId = browser1.Application.Process.Id;
			}

			var process = Process.GetProcessById(processId);
			using (var browser2 = Browser.AttachToBrowser(process))
			{
				Assert.IsNotNull(browser2);
				Assert.AreEqual(typeof(InternetExplorer), browser2.GetType());
			}
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			Browser.CloseBrowsers();
		}

		[TestMethod]
		public void CloseAllBrowsers()
		{
			using (var browser = InternetExplorer.Create())
			{
				Assert.IsNotNull(browser);
				Browser.CloseBrowsers(BrowserType.InternetExplorer);
				Thread.Sleep(50);
				Assert.IsFalse(Process.GetProcessesByName(InternetExplorer.BrowserName).Any());
			}
		}

		[TestMethod]
		public void CreateOneBrowser()
		{
			using (var browser = InternetExplorer.Create())
			{
				Assert.IsNotNull(browser);
			}
		}

		[TestMethod]
		public void CreateTwoBrowsers()
		{
			using (var browser1 = InternetExplorer.Create())
			{
				Assert.IsNotNull(browser1);
			}

			using (var browser2 = InternetExplorer.Create())
			{
				Assert.IsNotNull(browser2);
			}
		}

		[TestMethod]
		public void CreateTwoInstances()
		{
			using (var browser = InternetExplorer.Create())
			{
				using (var browser2 = InternetExplorer.Create())
				{
					var expected = "https://testr.local/Forms.html";
					Assert.IsNotNull(browser);
					browser.NavigateTo(expected);
					Assert.AreEqual(expected, browser.Uri);
					browser.MoveWindow(100, 110, 800, 600);
					Assert.AreEqual(100, browser.Window.Location.X);
					Assert.AreEqual(110, browser.Window.Location.Y);
					Assert.AreEqual(800, browser.Window.Size.Width);
					Assert.AreEqual(600, browser.Window.Size.Height);

					var expected2 = "https://testr.local/Forms2.html";
					Assert.IsNotNull(browser2);
					browser2.NavigateTo(expected2);
					Assert.AreEqual(expected2, browser2.Uri);
					browser2.MoveWindow(200, 220, 600, 480);
					Assert.AreEqual(200, browser2.Window.Location.X);
					Assert.AreEqual(220, browser2.Window.Location.Y);
					Assert.AreEqual(600, browser2.Window.Size.Width);
					Assert.AreEqual(480, browser2.Window.Size.Height);

					Assert.AreEqual(browser.Application.Process.Id, browser2.Application.Process.Id);
					Assert.AreNotEqual(browser.Window.Handle, browser2.Window.Handle);
				}
			}
		}

		[TestInitialize]
		public void TestInitialize()
		{
			Browser.CloseBrowsers();
		}

		#endregion
	}
}