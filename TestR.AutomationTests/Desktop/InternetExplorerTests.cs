#region References

using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.AutomationTests.Web;
using TestR.Web;
using TestR.Web.Browsers;
using TestR.Web.Elements;

#endregion

namespace TestR.AutomationTests.Desktop
{
	[TestClass]
	public class InternetExplorerTests
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

		[TestMethod]
		public void Children()
		{
			using (var browser = InternetExplorer.AttachOrCreate())
			{
				browser.NavigateTo(BrowserTests.TestSite);
				var watch = Stopwatch.StartNew();
				browser.Application.Refresh();
				watch.Stop();
				watch.Elapsed.Dump();
				Assert.IsTrue(watch.Elapsed.TotalMilliseconds < 500);
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
			Browser.CloseBrowsers(BrowserType.InternetExplorer);

			using (var browser = InternetExplorer.Create())
			{
				using (var browser2 = InternetExplorer.Create())
				{
					Assert.AreEqual(browser.Application.Process.Id, browser2.Application.Process.Id);
					Assert.AreNotEqual(browser.Window.Handle, browser2.Window.Handle);

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
					browser2.MoveWindow(900, 120, 900, 700);
					Assert.AreEqual(900, browser2.Window.Location.X);
					Assert.AreEqual(120, browser2.Window.Location.Y);
					Assert.AreEqual(900, browser2.Window.Size.Width);
					Assert.AreEqual(700, browser2.Window.Size.Height);
				}
			}
		}

		[TestMethod]
		public void ScrollingWithAttach()
		{
			Browser.CloseBrowsers(BrowserType.InternetExplorer);
			InternetExplorer.Create().Dispose();

			using (var browser = InternetExplorer.Attach())
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
		public void ScrollingWithCreate()
		{
			Browser.CloseBrowsers(BrowserType.InternetExplorer);

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

		[TestInitialize]
		public void TestInitialize()
		{
			Browser.CloseBrowsers();
		}

		#endregion
	}
}