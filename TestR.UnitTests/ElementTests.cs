﻿#region References

using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestR.Native;
using TestR.UnitTests.TestTypes;

#endregion

namespace TestR.UnitTests
{
	[TestClass]
	public class ElementTests
	{
		#region Methods

		[TestMethod]
		public void FirstNonGenericUsingFunction()
		{
			var host = TestHelper.CreateHost();
			var element = TestHelper.CreateElement("Parent", "Parent", host);
			var child = TestHelper.CreateElement("Child", "Child", element);
			element.Children.Add(child);
			Assert.AreEqual(1, element.Children.Count);

			var actual = element.First(x => x.Id == "Child");
			Assert.AreEqual(element.Children[0], actual);
			Assert.AreEqual(element, element.Children[0].Parent);
		}

		[TestMethod]
		public void FirstOrDefaultWithException()
		{
			var application = new Application((SafeProcess) null);
			var host = TestHelper.CreateMock<ElementHost>(application, null);
			var triggered = false;

			host.Object.Application.Timeout = TimeSpan.Zero;
			host.Setup(x => x.Refresh(It.IsAny<Func<Element, bool>>()))
				.Returns(() =>
				{
					triggered = true;
					throw new Exception("Boom");
				});

			var actual = host.Object.FirstOrDefault("Expected");
			Assert.IsNull(actual);
			Assert.IsTrue(triggered);
		}

		[TestMethod]
		public void FirstWhileWaiting()
		{
			var trigger = true;
			var expected = TestHelper.CreateElement("Expected", "Expected");
			var application = new Application((SafeProcess) null);
			var host = TestHelper.CreateMock<ElementHost>(application, null);

			host.Object.Application.Timeout = TimeSpan.FromSeconds(1);

			host.Setup(x => x.Refresh(It.IsAny<Func<Element, bool>>()))
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
		public void ParentSetViaConstructor()
		{
			var expected = TestHelper.CreateElement("Parent", "Parent");
			var element = new ElementOne("One", "One", expected);

			Assert.AreEqual(expected, element.Parent);
		}

		#endregion
	}
}