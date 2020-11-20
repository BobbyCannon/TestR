#region References

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace TestR.Tests
{
	public static class TestHelper
	{
		#region Constructors

		static TestHelper()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var path = Path.GetDirectoryName(assembly.Location);
			var info = new DirectoryInfo(path ?? "/");

			ApplicationPathForWinForms = info.FullName.Replace("TestR.Tests", "TestR.TestWinForms").Replace("netcoreapp3.1", "") + "TestR.TestWinForms.exe";
			ApplicationPathForWinFormX86 = info.FullName.Replace("TestR.Tests", "TestR.TestWinForms").Replace("netcoreapp3.1", "") + "TestR.TestWinForms-x86.exe";

			// C:\Workspaces\GitHub\TestR\TestR.Tests\bin\Debug\netcoreapp3.1
			// C:\Workspaces\GitHub\TestR\TestR.TestUwp\bin\Debug\AppX\TestR.TestUwp.exe
			ApplicationPathForUwp = info.FullName.Replace("TestR.Tests", "TestR.TestUwp").Replace("Debug\\netcoreapp3.1", "x86\\Debug\\AppX") + "\\TestR.TestUwp.exe";

			Application.CloseAll(ApplicationPathForWinForms);
			Application.CloseAll(ApplicationPathForWinFormX86);
		}

		#endregion

		#region Properties

		public static string ApplicationPathForUwp { get; set; }

		public static string ApplicationPathForWinForms { get; }

		public static string ApplicationPathForWinFormX86 { get; }

		#endregion

		#region Methods

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
			if (value is IEnumerable enumerable && value.GetType() != typeof(string))
			{
				if (value is byte[] bArray)
				{
					Console.Write("new byte[] { ");
					foreach (var x in bArray)
					{
						Console.Write($"0x{x:X2}, ");
					}
					Console.Write("};");
				}
				else
				{
					foreach (var x in enumerable)
					{
						x.Dump();
					}
				}
			}
			else
			{
				Console.WriteLine(string.IsNullOrWhiteSpace(label) ? value.ToString() : label + ":" + value);
			}
		}

		public static Application GetApplication(bool x86 = false)
		{
			var path = x86 ? ApplicationPathForWinFormX86 : ApplicationPathForWinForms;
			Application.CloseAll(path);
			var response = Application.AttachOrCreate(path);
			response.Timeout = TimeSpan.FromSeconds(5);
			response.AutoClose = true;
			return response;
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
		/// To literal version of the string.
		/// </summary>
		/// <param name="input"> The string input. </param>
		/// <returns> The literal version of the string. </returns>
		public static string ToLiteral(this string input)
		{
			var literal = new StringBuilder(input.Length);
			literal.Append("\"");
			foreach (var c in input)
			{
				switch (c)
				{
					case '\'':
						literal.Append(@"\'");
						break;
					case '\"':
						literal.Append("\\\"");
						break;
					case '\\':
						literal.Append(@"\\");
						break;
					case '\0':
						literal.Append(@"\0");
						break;
					case '\a':
						literal.Append(@"\a");
						break;
					case '\b':
						literal.Append(@"\b");
						break;
					case '\f':
						literal.Append(@"\f");
						break;
					case '\n':
						literal.Append(@"\n");
						break;
					case '\r':
						literal.Append(@"\r");
						break;
					case '\t':
						literal.Append(@"\t");
						break;
					case '\v':
						literal.Append(@"\v");
						break;
					default:
						// ASCII printable character
						if (c >= 0x20 && c <= 0x7e)
						{
							literal.Append(c);
							// As UTF16 escaped character
						}
						else
						{
							literal.Append(@"\u");
							literal.Append(((int) c).ToString("x4"));
						}

						break;
				}
			}

			literal.Append("\"");
			return literal.ToString();
		}

		/// <summary>
		/// To literal version of the byte array.
		/// </summary>
		/// <param name="input"> The byte array input. </param>
		/// <returns> The literal version of the data. </returns>
		public static string ToLiteral(this byte[] input)
		{
			var literal = new StringBuilder(input.Length);
			literal.Append("new [] { ");

			foreach (var c in input)
			{
				literal.Append($"0x{c:X2}, ");
			}

			literal.Remove(literal.Length - 2, 2);
			literal.Append(" }");
			return literal.ToString();
		}

		#endregion
	}
}