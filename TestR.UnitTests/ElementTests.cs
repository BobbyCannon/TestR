#region References

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestR.UnitTests.TestTypes;
using TestR.Web.Elements;

#endregion

namespace TestR.UnitTests
{
	[TestClass]
	public class ElementTests
	{
		#region Methods

		[TestMethod]
		public void GetWhileWaiting()
		{
			var trigger = true;
			var expected = TestHelper.CreateElement("Expected", "Expected");
			var application = new Application(null);
			var host = TestHelper.CreateMock<ElementHost>(application, null);
			var element = TestHelper.CreateElement("Root", "Root", host.Object);

			host.Object.Application.Timeout = TimeSpan.FromSeconds(1);

			host.Setup(x => x.Refresh())
				.Returns(() =>
				{
					trigger = !trigger;
					if (trigger)
					{
						host.Object.Children.Add(expected);
					}

					return host.Object;
				});

			var actual = host.Object.First(expected.Id);
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetNonGenericUsingFunction()
		{
			var host = TestHelper.CreateHost();
			var element = TestHelper.CreateElement("Parent", "Parent", host);
			element.Children.Add(TestHelper.CreateElement("Child", "Child", element));

			var actual = element.First(x => x.Id == "Child");
			Assert.AreEqual(element.Children[0], actual);
			Assert.AreEqual(element, element.Children[0].Parent);
		}


		[TestMethod]
		public void GetWithException()
		{
			var application = new Application(null);
			var host = TestHelper.CreateMock<ElementHost>(application, null);
			var triggered = false;

			host.Object.Application.Timeout = TimeSpan.Zero;
			host.Setup(x => x.Refresh())
				.Returns(() =>
				{
					triggered = true;
					throw new Exception("Boom");
				});

			var actual = host.Object.First("Expected");
			Assert.IsNull(actual);
			Assert.IsTrue(triggered);
		}

		[TestMethod]
		public void ParentSetViaConstructor()
		{
			var expected = TestHelper.CreateElement("Parent", "Parent");
			var element = new ElementOne("One", "One", expected);

			Assert.AreEqual(expected, element.Parent);
		}

		#endregion
	}
}