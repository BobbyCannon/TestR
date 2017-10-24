#region References

using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Native;

#endregion

namespace TestR.AutomationTests.Desktop
{
	[TestClass]
	public class MouseTests
	{
		#region Methods

		[TestMethod]
		public void MouseSelection()
		{
			Mouse.MoveTo(0, 0);
			Mouse.LeftClickDown();
			Thread.Sleep(50);
			Mouse.MoveTo(200, 300);
			Mouse.LeftClickUp();
		}

		[TestMethod]
		public void MouseSelectionRightButton()
		{
			Mouse.MoveTo(0, 0);
			Mouse.RightClickDown();
			Thread.Sleep(50);
			Mouse.MoveTo(200, 300);
			Mouse.RightClickUp();
		}

		[TestMethod]
		public void MouseSelectionRightButton2()
		{
			Mouse.MoveTo(200, 300);
			Mouse.RightClickDown();
			Thread.Sleep(50);
			Mouse.MoveTo(0, 0);
			Mouse.RightClickUp();
		}

		[TestMethod]
		public void MouseSelection2()
		{
			Mouse.Select(0, 0, 200, 400);
		}

		#endregion
	}
}