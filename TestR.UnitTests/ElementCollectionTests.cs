#region References

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.UnitTests.TestTypes;

#endregion

namespace TestR.UnitTests
{
	/// <summary>
	/// Summary description for ElementCollectionTests
	/// </summary>
	[TestClass]
	public class ElementCollectionTests
	{
		#region Methods

		[TestMethod]
		public void ContainsWithId()
		{
			var host = TestHelper.CreateHost();
			host.Children.Add(new ElementOne("ID", "ID", host), new ElementTwo("ID2", "ID2", host));

			var actual = host.Children.Contains(host.Children.First().Id);
			Assert.IsTrue(actual);
		}

		[TestMethod]
		public void ContainsWithReference()
		{
			var host = TestHelper.CreateHost();
			host.Children.Add(new ElementOne("ID", "ID", host),new ElementTwo("ID2", "ID2", host));
			
			var actual = host.Children.Contains(host.Children.First());
			Assert.IsTrue(actual);
		}

		[TestMethod]
		public void GetNonGenericUsingFunction()
		{
			var host = TestHelper.CreateHost();
			host.Children.Add(new ElementOne("ID", "ID", host), new ElementTwo("ID2", "ID2", host));
			

			var actual = host.Children.Get(x => x.Id == "ID");
			Assert.AreEqual(host.Children[0], actual);
		}

		[TestMethod]
		public void GetNonGenericUsingId()
		{
			var host = TestHelper.CreateHost();
			host.Children.Add(new ElementOne("ID", "ID", host), new ElementTwo("ID2", "ID2", host));
			

			var actual = host.Children.Get("ID");
			Assert.AreEqual(host.Children[0], actual);
		}

		[TestMethod]
		public void GetOfSpecificType()
		{
			var host = TestHelper.CreateHost();
			host.Children.Add(new ElementOne("ID", "ID", host), new ElementTwo("ID2", "ID2", host));

			var actual = host.Children.First<ElementTwo>("ID2");
			Assert.AreEqual(host.Children.Skip(1).First(), actual);
		}

		[TestMethod]
		public void GetOfSpecificTypeWithDuplicateId()
		{
			var host = TestHelper.CreateHost();
			host.Children.Add(new ElementOne("One", "One", host),
				new ElementTwo("Two", "Two", host),
				new ElementTwo("Three", "Three", host),
				new ElementTwo("Two", "Two", host),
				new ElementOne("One2", "One2", host));

			var actual = host.Children.First<ElementTwo>("Two");
			Assert.AreEqual(host.Children.Skip(1).First(), actual);
		}

		[TestMethod]
		public void GetOfSpecificTypeWithInvalidId()
		{
			var host = TestHelper.CreateHost();
			host.Children.Add(new ElementOne("One", "One", host), new ElementTwo("Two", "Two", host));

			var actual = host.Children.First<ElementTwo>("One", false);
			Assert.IsNull(actual);
		}

		[TestMethod]
		public void GetWithIncludeDescendantsDefaultValueOfTrue()
		{
			var host = TestHelper.CreateHost();
			var parent = new ElementOne("One", "One", host);
			var expected = new ElementOne("One2", "One2", host);
			parent.Children.Add(expected);
			
			host.Children.Add(parent);
			host.Children.Add(new ElementTwo("Two", "Two", host));

			var actual = host.Children.First<ElementOne>("One2");
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetWithInvalidId()
		{
			var host = TestHelper.CreateHost();
			host.Children.Add(new ElementOne("ID", "ID", host), new ElementTwo("ID2", "ID2", host));

			var actual = host.Children.First<ElementTwo>("ID3");
			Assert.IsNull(actual);
		}

		[TestMethod]
		public void ParentSetViaConstructor()
		{
			var host = TestHelper.CreateHost();
			Assert.AreEqual(host, host.Children.Parent);
		}

		[TestMethod]
		public void GetAllWithGeneric()
		{
			var host = TestHelper.CreateHost();
			host.Children.Add(new ElementOne("E1", "E1", host));
			host.Children.Add(new ElementTwo("E2", "E2", host));
			host.Children.Add(new ElementOne("E1.1", "E1.1", host));
			host.Children.Add(new ElementTwo("E2.1", "E2.1", host));

			var expected = new[] { (ElementOne) host.Children[0], (ElementOne) host.Children[2] };
			var actual = host.Children.GetAll<ElementOne>(x => true).ToArray();

			TestHelper.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetAllWithGenericAndDescendants()
		{
			var host = TestHelper.CreateHost();
			var parent1 = new ElementOne("E1", "E1", host);
			var parent1Child1 = new ElementOne("E1C1", "E1C1", parent1);
			parent1Child1.Children.Add(new ElementTwo("E1C1G2", "E1C1G2", parent1Child1));
			parent1Child1.Children.Add(new ElementOne("E1C1G1", "E1C1G1", parent1Child1));
			parent1.Children.Add(parent1Child1);
			host.Children.Add(parent1);
			host.Children.Add(new ElementTwo("E2", "E2", host));
			var parent2 = new ElementOne("E1.1", "E1.1", host);
			parent2.Children.Add(new ElementOne("E1.1C1", "E1.1C1", parent2));
			host.Children.Add(parent2);
			host.Children.Add(new ElementTwo("E2.1", "E2.1", host));

			var expected = new[]
			{
				(ElementOne) host.Children[0],
				(ElementOne) host.Children[2],
				(ElementOne) host.Children[0].Children[0],
				(ElementOne) host.Children[0].Children[0].Children[1],
				(ElementOne) host.Children[2].Children[0]
			};

			var actual = host.Children.GetAll<ElementOne>(x => true).ToArray();
			TestHelper.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetAllWithGenericAndDescendantsWithCondition()
		{
			var host = TestHelper.CreateHost();
			var parent1 = new ElementOne("E1", "E1", host);
			var parent1Child1 = new ElementOne("E1C1", "E1C1", parent1);
			parent1Child1.Children.Add(new ElementTwo("E1C1G2", "E1C1G2", parent1Child1));
			parent1Child1.Children.Add(new ElementOne("E1C1G1", "E1C1G1", parent1Child1));
			parent1.Children.Add(parent1Child1);
			host.Children.Add(parent1);
			host.Children.Add(new ElementTwo("E2", "E2", host));
			var parent2 = new ElementOne("E1.1", "E1.1", host);
			parent2.Children.Add(new ElementOne("E1.1C1", "E1.1C1", parent2));
			host.Children.Add(parent2);
			host.Children.Add(new ElementTwo("E2.1", "E2.1", host));

			var expected = new[]
			{
				(ElementOne) host.Children[0].Children[0].Children[1],
			};

			var actual = host.Children.GetAll<ElementOne>(x => x.Id == "E1C1G1").ToArray();
			TestHelper.AreEqual(expected, actual);
		}

		#endregion
	}
}