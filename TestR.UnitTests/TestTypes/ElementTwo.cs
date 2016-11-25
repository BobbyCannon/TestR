using System;

namespace TestR.UnitTests.TestTypes
{
	public class ElementTwo : BaseElement
	{
		#region Constructors

		public ElementTwo(string id, string name, TimeSpan timeout, BaseElement parent) 
			: base(id, name, timeout, parent)
		{
		}

		#endregion
	}
}