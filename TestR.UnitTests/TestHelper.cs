#region References

using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
			var application = new Application(null);
			var response = CreateMock<ElementHost>(application, null);

			return response.Object;
		}

		public static Mock<T> CreateMock<T>(params object[] arguments) where T : class
		{
			return new Mock<T>(arguments);
		}

		#endregion
	}
}