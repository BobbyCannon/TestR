#region References

using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Native;

#endregion

namespace TestR.AutomationTests.UWP
{
	[TestClass]
	public class UniversalTests
	{
		#region Methods

		[TestMethod]
		public void TestMethod1()
		{
			var applicationName = "TestR.TestUniversal.exe";
			var packageFamilyName = "f72c1fbb-d462-45eb-ac36-6523dc784980_1myjjx7jt00bj";
			
			//var p = new Process();
			//var startInfo = new ProcessStartInfo();
			//startInfo.UseShellExecute = true;
			////startInfo.FileName = path;
			//startInfo.FileName = @"shell:appsFolder\f72c1fbb-d462-45eb-ac36-6523dc784980_1myjjx7jt00bj!App";
			//p.StartInfo = startInfo;
			//p.Start();

			using (var app = Application.AttachOrCreateUniversal(applicationName, packageFamilyName))
			{
				app.Id.Dump();
				app.Descendants().Count().Dump();
			}

			//ProcessService.Where(x => true).Select(x => x.FileName).Dump();
		}

		#endregion
	}
}