#region References

using System;
using System.Management.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Extensions;
using TestR.PowerShell;
using TestR.Web;
using TestR.Web.Browsers;

#endregion

namespace TestR.IntegrationTests.Web
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "Chrome")]
	public class ChromeTests : TestCmdlet
	{
		#region Methods

		[TestMethod]
		public void Attach()
		{
			Browser.CloseBrowsers();
			using (var browser = Chrome.Create())
			{
				Assert.IsNotNull(browser);
			}

			using (var browser = Chrome.Attach())
			{
				Assert.IsNotNull(browser);
				Console.WriteLine(browser.Id);
				browser.NavigateTo("http://bing.com");
				browser.Elements.Count.Dump();
				browser.ExecuteScript("window.location.href").Dump();
			}
		}

		[TestMethod]
		public void AttachOrCreate()
		{
			Browser.CloseBrowsers();

			using (var browser = Chrome.AttachOrCreate())
			{
				Assert.IsNotNull(browser);
				Console.WriteLine(browser.Id);
				browser.NavigateTo("http://bing.com");
				browser.Elements.Count.Dump();
				browser.ExecuteScript("window.location.href").Dump();
			}
		}

		[TestMethod]
		public void Create()
		{
			Browser.CloseBrowsers();

			using (var browser = Chrome.Create())
			{
				Assert.IsNotNull(browser);
				Console.WriteLine(browser.Id);
				browser.NavigateTo("http://bing.com");
				browser.Elements.Count.Dump();
				browser.ExecuteScript("window.location.href").Dump();
			}
		}

		#endregion
	}
}