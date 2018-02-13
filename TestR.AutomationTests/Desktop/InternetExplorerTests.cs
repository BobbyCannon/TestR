#region References

using System;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop.Elements;
using TestR.PowerShell;
using TestR.Web;
using TestR.Web.Browsers;
using TestR.Web.Elements;

#endregion

namespace TestR.AutomationTests.Desktop
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "InternetExplorer")]
	public class InternetExplorerTests : TestCmdlet
	{
		#region Methods

		[TestMethod]
		public void Scrolling()
		{
			using (var browser = InternetExplorer.AttachOrCreate())
			{
				var body = browser.FirstOrDefault<Body>();
				var builder = new StringBuilder();

				for (var i = 0; i < 1000; i++)
				{
					builder.AppendLine($"Item {i}<br />");
				}

				body.SetHtml(builder.ToString());

				for (var i = 0; i <= 75; i++)
				{
					browser.Scroll(0, i);
					Thread.Sleep(10);

					Assert.AreEqual(0, browser.HorizontalScrollPercent);
					Assert.IsTrue(browser.VerticalScrollPercent >= i - 1 && browser.VerticalScrollPercent <= i + 1);
				}

				builder.AppendLine("aoeu");
				body.SetHtml(builder.ToString());
				browser.Scroll(0, 100);
				Assert.AreEqual(0, browser.HorizontalScrollPercent);
				Assert.AreEqual(100, browser.VerticalScrollPercent);
			}
		}

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
					browser.MoveWindow(100, 110, 400, 300);
					Assert.AreEqual(100, browser.Window.Location.X);
					Assert.AreEqual(110, browser.Window.Location.Y);
					Assert.AreEqual(400, browser.Window.Size.Width);
					Assert.AreEqual(300, browser.Window.Size.Height);

					var expected2 = "https://testr.local/Forms2.html";
					Assert.IsNotNull(browser2);
					browser2.NavigateTo(expected2);
					Assert.AreEqual(expected2, browser2.Uri);
					browser2.MoveWindow(500, 120, 500, 400);
					Assert.AreEqual(500, browser2.Window.Location.X);
					Assert.AreEqual(120, browser2.Window.Location.Y);
					Assert.AreEqual(500, browser2.Window.Size.Width);
					Assert.AreEqual(400, browser2.Window.Size.Height);

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