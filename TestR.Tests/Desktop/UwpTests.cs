#region References

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop.Elements;

#endregion

namespace TestR.Tests.Desktop
{
	[TestClass]
	public class UwpTests
	{
		#region Methods

		[TestMethod]
		public void LaunchUwpApp()
		{
			var filepath = TestHelper.ApplicationPathForUwp;
			var packageName = "5495f851-725d-41f7-b34a-889a800e2315_1myjjx7jt00bj";
			var application = Application.AttachOrCreateUniversal(filepath, packageName);

			application.Children.Count.Dump();
			var children = application.Descendants().ToList();
			children.Count.Dump();
			children.ForEach(x =>
			{
				x.FullId.Dump();
				x.GetType().Dump();
			});

			application.FirstOrDefault<Edit>("FirstInput").SetText(DateTime.UtcNow.ToString());
			application.BringToFront();
			
			application.Dispose();
		}

		#endregion
	}
}