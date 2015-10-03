#region References

using System;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop;
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

		#endregion
	}
}