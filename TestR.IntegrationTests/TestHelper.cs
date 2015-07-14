#region References

using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Logging;

#endregion

namespace TestR.IntegrationTests
{
	public static class TestHelper
	{
		#region Methods

		public static void AddConsoleLogger()
		{
			if (LogManager.Loggers.Count <= 0)
			{
				LogManager.Loggers.Add(new ConsoleLogger { Level = LogLevel.Verbose });
			}
		}

		public static void AreEqual<T>(T expected, T actual)
		{
			var compareObjects = new CompareLogic();
			compareObjects.Config.MaxDifferences = int.MaxValue;

			var result = compareObjects.Compare(expected, actual);
			Assert.IsTrue(result.AreEqual, result.DifferencesString);
		}

		#endregion
	}
}