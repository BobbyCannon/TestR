#region References

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop;

#endregion

namespace TestR.Tests
{
	[TestClass]
	public class InputBuilderTests
	{
		#region Methods

		[TestMethod]
		public void name()
		{
			var builder = new InputBuilder();
			builder.AddKeyPress(KeyboardKey.A);
		}

		#endregion
	}
}