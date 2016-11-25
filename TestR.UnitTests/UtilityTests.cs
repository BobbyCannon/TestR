#region References

using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
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
		public void DefaultWaitDelay()
		{
			Assert.AreEqual(50, Utility.DefaultWaitDelay);
		}

		[TestMethod]
		public void DefaultWaitTimeout()
		{
			Assert.AreEqual(1000, Utility.DefaultWaitTimeout);
		}

		[TestMethod]
		public void WaitWithFailingActionAndNoTimeout()
		{
			var actual = Utility.Wait(() => false, 0);
			Assert.IsFalse(actual);
		}

		[TestMethod]
		public void WaitWithFailingActionAndTimeoutThenPassingAction()
		{
			var value = true;
			var watch = Stopwatch.StartNew();
			var actual = Utility.Wait(() => value = !value, int.MaxValue);
			Assert.IsTrue(actual);
			Assert.IsTrue(watch.ElapsedMilliseconds >= Utility.DefaultWaitDelay);
			Assert.IsTrue(watch.ElapsedMilliseconds < Utility.DefaultWaitDelay * 2);
		}

		[TestMethod]
		public void WaitWithPassingAction()
		{
			var actual = Utility.Wait(() => true);
			Assert.IsTrue(actual);
		}

		[TestMethod]
		public void WaitWithProvidedInput()
		{
			var expected = new ElementOne("1", "1", TimeSpan.MinValue, null);
			ElementOne actualInput = null;

			var actual = Utility.Wait(expected, x =>
			{
				actualInput = x;
				return true;
			});

			Assert.IsTrue(actual);
			Assert.AreEqual(expected, actualInput);
		}

		#endregion
	}
}