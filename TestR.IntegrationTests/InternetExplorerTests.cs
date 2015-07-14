#region References

using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.PowerShell;
using TestR.Web;
using TestR.Web.Browsers;

#endregion

namespace TestR.IntegrationTests
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "InternetExplorer")]
	public class InternetExplorerTests : TestCmdlet
	{
		#region Methods

		[TestMethod]
		public void AttachOneBrowserShouldSucceed()
		{
			Browser.CloseBrowsers(BrowserType.InternetExplorer);

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
		public void AttachOneBrowserWithTwoCreatedShouldSucceed()
		{
			Browser.CloseBrowsers(BrowserType.InternetExplorer);

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
		public void AttachOrCreateOneBrowserShouldSucceed()
		{
			Browser.CloseBrowsers(BrowserType.InternetExplorer);

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
		public void AttachOrCreateShouldSucceed()
		{
			Browser.CloseBrowsers(BrowserType.InternetExplorer);

			using (var browser = InternetExplorer.AttachOrCreate())
			{
				Assert.IsNotNull(browser);
			}
		}

		[TestMethod]
		public void CloseAllBrowsers()
		{
			using (var browser = InternetExplorer.Create())
			{
				browser.Application.WaitWhileBusy();
				Thread.Sleep(1000);

				Browser.CloseBrowsers(BrowserType.InternetExplorer);
				Assert.IsFalse(Process.GetProcessesByName(InternetExplorer.Name).Any());
			}
		}

		[TestMethod]
		public void CreateOneBrowserShouldSucceed()
		{
			Browser.CloseBrowsers(BrowserType.InternetExplorer);

			using (var browser = InternetExplorer.Create())
			{
				Assert.IsNotNull(browser);
			}
		}

		[TestMethod]
		public void CreateTwoBrowsersShouldSucceed()
		{
			Browser.CloseBrowsers(BrowserType.InternetExplorer);

			using (var browser1 = InternetExplorer.Create())
			{
				Assert.IsNotNull(browser1);
			}

			using (var browser2 = InternetExplorer.Create())
			{
				Assert.IsNotNull(browser2);
			}
		}

		#endregion
	}
}