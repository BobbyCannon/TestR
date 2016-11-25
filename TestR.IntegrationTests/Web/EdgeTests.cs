#region References

using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Extensions;
using TestR.PowerShell;
using TestR.Web;
using TestR.Web.Browsers;

#endregion

namespace TestR.AutomationTests.Web
{
	//[TestClass]
	//[Cmdlet(VerbsDiagnostic.Test, "Edge")]
	public class EdgeTests : TestCmdlet
	{
		#region Methods

		[TestMethod]
		public void Attach()
		{
			using (var browser = Edge.Create())
			{
				Assert.IsNotNull(browser);
			}

			using (var browser = Edge.Attach())
			{
				Assert.IsNotNull(browser);
				Console.WriteLine(browser.Id);
				browser.NavigateTo("http://localhost:8080");
				browser.Elements.Count.Dump();
				browser.ExecuteScript("window.location.href").Dump();
			}
		}

		[TestMethod]
		public void AttachOrCreate()
		{
			using (var browser = Edge.AttachOrCreate())
			{
				Assert.IsNotNull(browser);
				Console.WriteLine(browser.Id);
				browser.NavigateTo("http://localhost:8080");
				browser.Elements.Count.Dump();
				browser.ExecuteScript("window.location.href").Dump();
			}
		}

		[TestMethod]
		public void AttachToBrowser()
		{
			int processId;

			using (var browser1 = Edge.Create())
			{
				Assert.IsNotNull(browser1);
				processId = browser1.Application.Process.Id;
			}

			var process = Process.GetProcessById(processId);
			using (var browser2 = Browser.AttachToBrowser(process))
			{
				Assert.IsNotNull(browser2);
				Assert.AreEqual(typeof(Edge), browser2.GetType());
			}
		}

		[TestCleanup]
		public void Cleanup()
		{
			Browser.CloseBrowsers();
		}

		[TestMethod]
		public void Create()
		{
			using (var browser = Edge.Create())
			{
				Assert.IsNotNull(browser);
				Console.WriteLine(browser.Id);
				browser.NavigateTo("http://localhost:8080");
				browser.Elements.Count.Dump();
				browser.ExecuteScript("window.location.href").Dump();
			}
		}

		[TestInitialize]
		public void Initialize()
		{
			Browser.CloseBrowsers();
		}

		#endregion
	}
}