#region References

using System;
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
		public void GetOfSpecificType()
		{
			var collection = new ElementCollection<BaseElement>
			{
				new ElementOne("ID", "ID", TimeSpan.Zero, null),
				new ElementTwo("ID", "ID", TimeSpan.Zero, null)
			};

			var actual = collection.Get<ElementTwo>("ID");
			Assert.AreEqual(collection[1], actual);
		}

		[TestMethod]
		public void GetOfSpecificTypeWithDuplicateId()
		{
			var collection = new ElementCollection<BaseElement>
			{
				new ElementOne("One", "One", TimeSpan.Zero, null),
				new ElementTwo("Two", "Two", TimeSpan.Zero, null),
				new ElementTwo("Three", "Three", TimeSpan.Zero, null),
				new ElementTwo("Two", "Two", TimeSpan.Zero, null),
				new ElementOne("One2", "One2", TimeSpan.Zero, null)
			};

			var actual = collection.Get<ElementTwo>("Two");
			Assert.AreEqual(collection[1], actual);
		}

		[TestMethod]
		public void GetOfSpecificTypeWithInvalidId()
		{
			var collection = new ElementCollection<BaseElement>
			{
				new ElementOne("One", "One", TimeSpan.Zero, null),
				new ElementTwo("Two", "Two", TimeSpan.Zero, null)
			};

			var actual = collection.Get<ElementTwo>("One", false);
			Assert.IsNull(actual);
		}

		[TestMethod]
		public void GetWithIncludeDescendantsDefaultValueOfTrue()
		{
			var parent = new ElementOne("One", "One", TimeSpan.Zero, null);
			var expected = new ElementOne("One2", "One2", TimeSpan.Zero, parent);
			parent.Children.Add(expected);

			var collection = new ElementCollection<BaseElement>
			{
				parent,
				new ElementTwo("Two", "Two", TimeSpan.Zero, null)
			};

			var actual = collection.Get<ElementOne>("One2");
			Assert.AreEqual(expected, actual);
		}

		#endregion
	}
}