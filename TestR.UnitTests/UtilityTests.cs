#region References

using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.UnitTests.TestTypes;

#endregion

namespace TestR.UnitTests
{
	[TestClass]
	public class UtilityTests
	{
		#region Methods

		[TestMethod]
		public void WaitWithFailingActionAndNoTimeout()
		{
			var actual = Utility.Wait(() => false, 0, 50);
			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void WaitWithDelayValue()
		{
			var value = true;
			var watch = Stopwatch.StartNew();
			var actual = Utility.Wait(() => value = !value, int.MaxValue, 50);
			Assert.IsTrue(actual);
			Assert.IsTrue(watch.ElapsedMilliseconds >= 50);
			Assert.IsTrue(watch.ElapsedMilliseconds < 50 * 2);

			actual = Utility.Wait(() => value = !value, int.MaxValue, 200);
			Assert.IsTrue(actual);
			Assert.IsTrue(watch.ElapsedMilliseconds >= 200);
			Assert.IsTrue(watch.ElapsedMilliseconds < 200 * 2);
		}

		[TestMethod]
		public void WaitWithPassingAction()
		{
			var watch = Stopwatch.StartNew();
			var actual = Utility.Wait(() => true, int.MaxValue, 1000);
			Assert.IsTrue(actual);
			Assert.IsTrue(watch.ElapsedMilliseconds < 1000);
		}

		[TestMethod]
		public void WaitWithFailingActionWithSecondDelay()
		{
			var watch = Stopwatch.StartNew();
			var actual = Utility.Wait(() => false, 1000, 10);
			Assert.IsFalse(actual);
			Assert.IsTrue(watch.ElapsedMilliseconds >= 1000);
		}

		[TestMethod]
		public void WaitWithProvidedInput()
		{
			var host = TestHelper.CreateHost();
			var expected = new ElementOne("1", "1", host);
			Element actualInput = null;

			var actual = Utility.Wait(expected, x =>
			{
				actualInput = x;
				return true;
			}, 1000, 50);

			Assert.IsTrue(actual);
			Assert.AreEqual(expected, actualInput);
		}

		#endregion
	}
}