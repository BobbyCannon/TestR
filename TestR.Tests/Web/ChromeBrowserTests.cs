#region References

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Web.Browsers;

#endregion

namespace TestR.Tests.Web
{
	[TestClass]
	public class ChromeBrowserTests
	{
		#region Methods

		[TestMethod]
		public void CreateBrowser()
		{
			using var browser = Chrome.AttachOrCreate();
		}

		#endregion
	}
}