#region References

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop.Elements;

#endregion

namespace TestR.AutomationTests.Desktop.Elements
{
	[TestClass]
	public class ComboBoxTests : BaseTest
	{
		#region Methods

		[TestMethod]
		public void Text()
		{
			using (var application = GetApplication())
			{
				var comboBox = application.First<ComboBox>("comboBox1");
				Assert.IsNotNull(comboBox);
				comboBox.TypeText("One");
				Assert.AreEqual("One", comboBox.Text);
			}
		}

		#endregion
	}
}