#region References

using System;
using System.Diagnostics;
using System.Threading;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace TestR.AutomationTests
{
	public static class TestHelper
	{
		#region Constants

		public const int DefaultRetryDelay = 50;

		public const double DefaultRetryTimeout = 1000;

		#endregion

		#region Methods

		public static void AddConsoleLogger()
		{
			//if (LogManager.Loggers.Count <= 0)
			//{
			//	LogManager.Loggers.Add(new ConsoleLogger { Level = LogLevel.Verbose });
			//}
		}

		public static void AreEqual<T>(T expected, T actual)
		{
			var compareObjects = new CompareLogic { Config = { MaxDifferences = int.MaxValue } };

			var result = compareObjects.Compare(expected, actual);
			Assert.IsTrue(result.AreEqual, result.DifferencesString);
		}

		/// <summary>
		/// Dumps the object to Console.WriteLine.
		/// </summary>
		/// <param name="value"> The value to dump to the console. </param>
		/// <param name="label"> The label to prefix the value. </param>
		public static void Dump(this object value, string label = "")
		{
			Console.WriteLine(string.IsNullOrWhiteSpace(label) ? value.ToString() : label + ":" + value);
		}

		public static void PrintChildren(Element parent, string prefix = "")
		{
			var element = parent;
			if (element != null)
			{
				Console.WriteLine(prefix + element.ToDetailString().Replace(Environment.NewLine, ", "));
				prefix += "  ";
			}

			foreach (var child in parent.Children)
			{
				PrintChildren(child, prefix);
			}
		}

		/// <summary>
		/// Continues to run the action until we hit the timeout. If an exception occurs then delay for the
		/// provided delay time.
		/// </summary>
		/// <param name="action"> The action to attempt to retry. </param>
		/// <param name="timeout"> The timeout to stop retrying. </param>
		/// <param name="delay"> The delay between retries. </param>
		/// <returns> The response from the action. </returns>
		public static void Retry(Action action, double timeout = DefaultRetryTimeout, int delay = DefaultRetryDelay)
		{
			Retry<object>(() =>
			{
				action();
				return null;
			}, timeout, delay);
		}

		internal static void AreEqual<T>(T expected, Func<T> actual, int timeout, int delay = 100)
		{
			Retry(() => AreEqual(expected, actual()), timeout, delay);
		}

		/// <summary>
		/// Continues to run the action until we hit the timeout. If an exception occurs then delay for the
		/// provided delay time.
		/// </summary>
		/// <param name="action"> The action to attempt to retry. </param>
		/// <param name="timeout"> The timeout to stop retrying. </param>
		/// <param name="delay"> The delay between retries. </param>
		/// <returns> The response from the action. </returns>
		private static T Retry<T>(Func<T> action, double timeout = DefaultRetryTimeout, int delay = DefaultRetryDelay)
		{
			var watch = Stopwatch.StartNew();

			try
			{
				return action();
			}
			catch (Exception)
			{
				Thread.Sleep(delay);

				var remaining = timeout - watch.Elapsed.TotalMilliseconds;
				if (remaining <= 0)
				{
					throw;
				}

				return Retry(action, remaining, delay);
			}
		}

		#endregion
	}
}