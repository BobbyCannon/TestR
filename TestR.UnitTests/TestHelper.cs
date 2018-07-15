#region References

using System.Text;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestR.Native;

#endregion

namespace TestR.UnitTests
{
	public static class TestHelper
	{
		#region Methods

		public static void AreEqual<T>(T expected, T actual)
		{
			var compareObjects = new CompareLogic
			{
				Config = { MaxDifferences = int.MaxValue }
			};

			var result = compareObjects.Compare(expected, actual);
			Assert.IsTrue(result.AreEqual, result.DifferencesString);
		}

		public static Element CreateElement(string id, string name, ElementHost host = null)
		{
			var myHost = host ?? CreateHost();
			var response = new Mock<Element>(myHost.Application, myHost);

			response.SetupGet(x => x.Id).Returns(() => id);
			response.SetupGet(x => x.Name).Returns(() => name);

			return response.Object;
		}

		public static ElementHost CreateHost()
		{
			var application = new Application((SafeProcess) null);
			var response = CreateMock<ElementHost>(application, null);

			return response.Object;
		}

		public static Mock<T> CreateMock<T>(params object[] arguments) where T : class
		{
			return new Mock<T>(arguments);
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