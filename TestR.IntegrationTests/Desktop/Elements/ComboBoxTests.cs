#region References

using System.IO;
using System.Management.Automation;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop.Elements;
using TestR.PowerShell;

#endregion

namespace TestR.AutomationTests.Desktop.Elements
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "ComboBox")]
	public class ComboBoxTests : TestCmdlet
	{
		#region Fields

		public static string ApplicationPath;

		#endregion

		#region Methods

		[TestInitialize]
		public void Setup()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var path = Path.GetDirectoryName(assembly.Location);
			var info = new DirectoryInfo(path ?? "/");

			ApplicationPath = info.Parent?.Parent?.Parent?.FullName;
			ApplicationPath += "\\TestR.TestWinForms\\Bin\\" + (assembly.IsAssemblyDebugBuild() ? "Debug" : "Release") + "\\TestR.TestWinForms.exe";
			Application.CloseAll(ApplicationPath);
		}

		[TestMethod]
		public void Text()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var comboBox = application.Get<ComboBox>("comboBox1");
				Assert.IsNotNull(comboBox);
				comboBox.TypeText("One");
				Assert.AreEqual("One", comboBox.Text);
			}
		}

		#endregion
	}
}