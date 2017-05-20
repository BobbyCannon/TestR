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

namespace TestR.AutomationTests.Web
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
		public void AttachOneBrowserWithTwoCreated()
		{
			using (var browser1 = InternetExplorer.Create())
			{
				Assert.IsNotNull(browser1);
			}

			using (var browser2 = InternetExplorer.Create())
			{
				Assert.IsNotNull(browser2);
			}

			using (var browser3 = InternetExplorer.Attach())
			{
				Assert.IsNotNull(browser3);
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
				browser.Application.WaitForComplete();
				Thread.Sleep(1000);

				Browser.CloseBrowsers(BrowserType.InternetExplorer);
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
		public void CreateTwoInstances()
		{
			using (var browser = InternetExplorer.Create())
			{
				using (var browser2 = InternetExplorer.Create())
				{
					var expected = $"https://{Environment.MachineName.ToLower()}/";
					Assert.IsNotNull(browser);
					Console.WriteLine(browser.Id);
					browser.NavigateTo(expected);
					browser.ExecuteScript("window.location.href").Dump();
					Assert.AreEqual(expected, browser.Uri);

					Assert.IsNotNull(browser2);
					Console.WriteLine(browser2.Id);
					browser2.NavigateTo(expected);
					browser2.ExecuteScript("window.location.href").Dump();
					Assert.AreEqual(expected, browser2.Uri);

					Assert.AreNotEqual(browser.Id, browser2.Id);
				}
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

		[TestInitialize]
		public void TestInitialize()
		{
			Browser.CloseBrowsers();
		}

		#endregion
	}
}