#region References

using System;
using System.Collections;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace TestR.AutomationTests
{
	public static class TestHelper
	{
		#region Methods

		public static void AddConsoleLogger()
		{
			//if (LogManager.Loggers.Count <= 0)
			//{
			//	LogManager.Loggers.Add(new ConsoleLogger { Level = LogLevel.Verbose });
			//}
		}

		public static void AreEqual<T>(T expected, T actual, bool ignoreOrder = true)
		{
			var compareObjects = new CompareLogic { Config = { MaxDifferences = int.MaxValue, IgnoreCollectionOrder = ignoreOrder } };
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
			var enumerable = value as IEnumerable;
			if (enumerable != null && value.GetType() != typeof(string))
			{
				foreach (var x in enumerable)
				{
					x.Dump();
				}
			}
			else
			{
				Console.WriteLine(string.IsNullOrWhiteSpace(label) ? value.ToString() : label + ":" + value);
			}
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