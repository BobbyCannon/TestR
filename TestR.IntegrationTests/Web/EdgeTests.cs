#region References

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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
				browser.NavigateTo("http://bing.com");
				Thread.Sleep(1500);
				//browser.ExecuteScript(Browser.GetTestScript()).Dump();
				//browser.ExecuteScript("TestR = { autoId: 1 };");
				//browser.ExecuteScript("test = { id: 1};").Dump();
				//browser.ExecuteScript("console.log(test);").Dump();
				//browser.ExecuteScript("console.log(TestR.autoId);").Dump();
				browser.ExecuteScript("window.location.href").Dump();
				//browser.Test().Dump();
			}
		}

		[TestMethod]
		public void UnitOfWork()
		{
			var response = JsonConvert.DeserializeObject<dynamic>("{\"sessionId\":\"EAFFFD51 - 72AD - 4900 - B152 - 342F07EC6FB0\",\"status\":0,\"value\":{\"ELEMENT\":\"4a598a23 - 7a23 - 4e4a - a9ee - d35e787a0a9f\",\"element - 6066 - 11e4 - a52e - 4f735466cecf\":\"4a598a23 - 7a23 - 4e4a - a9ee - d35e787a0a9f\"}}");
			((object)response.value.ELEMENT.ToString()).Dump();

			var blah = "http://localhost:17556/session/898A8188-EF03-4EF7-9AE1-BF133AA8023B/element/1c95d5dc-5ffe-43c7-934f-f7877b756b4e/attribute/value";
		}

		#endregion
	}
}