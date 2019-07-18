using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Native;

namespace TestR.AutomationTests.Desktop
{
	[TestClass]
	public class ProcessServiceTests
	{
		[TestMethod]
		public void WhereShouldBeFast()
		{
			var notepadPath = @"C:\Windows\Notepad.exe";
			Application.CloseAll(notepadPath);

			var processes = ProcessService.Where("Notepad.exe").ToList();
			Assert.AreEqual(0, processes.Count);

			using (var a = Application.Create(notepadPath))
			{
				var watch = Stopwatch.StartNew();
				processes = ProcessService.Where("Notepad.exe").ToList();
				watch.Stop();
				watch.Elapsed.Dump();
				a.Close();

				Assert.AreEqual(1, processes.Count);
				Assert.IsTrue(watch.Elapsed.TotalMilliseconds < 250);
			}

		}
	}
}
