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
	[Cmdlet(VerbsDiagnostic.Test, "Edge")]
	public class EdgeTests : TestCmdlet
	{
		#region Methods

		[TestMethod]
		public void Attach()
		{
			Browser.CloseBrowsers();
			using (var browser = Edge.Create())
			{
				Assert.IsNotNull(browser);
			}

			using (var browser = Edge.Attach())
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

			using (var browser = Edge.AttachOrCreate())
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

			using (var browser = Edge.Create())
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