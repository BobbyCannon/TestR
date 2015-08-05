#region References

using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Extensions;
using TestR.Web;
using TestR.Web.Browsers;

#endregion

namespace TestR.IntegrationTests.Web
{
	[TestClass]
	public class EdgeTests
	{
		#region Methods

		[TestMethod]
		public void Create()
		{
			using (Edge browser = Edge.Create())
			{
				Console.WriteLine(browser.Id);
				browser.NavigateTo("http://bobbycannon.com");
				//Thread.Sleep(1500);
				//browser.ExecuteScript(Browser.GetTestScript()).Dump();
				//browser.ExecuteScript("console.log('test');").Dump();
				//browser.ExecuteScript("console.log(\"test2\");").Dump();
				browser.ExecuteScript("window.location.href").Dump();
				browser.Test().Dump();
			}
		}

		#endregion
	}
}