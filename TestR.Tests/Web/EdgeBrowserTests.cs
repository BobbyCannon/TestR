#region References

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Web.Browsers;

#endregion

namespace TestR.Tests.Web
{
	[TestClass]
	public class EdgeBrowserTests
	{
		#region Methods

		[TestMethod]
		public void CreateBrowser()
		{
			using var browser = Edge.AttachOrCreate();
			browser.NavigateTo("about:blank");
		}

		#endregion
	}
}