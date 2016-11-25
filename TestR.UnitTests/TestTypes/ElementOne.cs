#region References

using System;

#endregion

namespace TestR.UnitTests.TestTypes
{
	public class ElementOne : BaseElement
	{
		#region Constructors

		public ElementOne(string id, string name, TimeSpan timeout, BaseElement parent) 
			: base(id, name, timeout, parent)
		{
		}

		#endregion
	}
}