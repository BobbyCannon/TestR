#region References

using System.IO;
using System.Management.Automation;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop;
using TestR.Extensions;

#endregion

namespace TestR.IntegrationTests.Desktop.Elements
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "ComboBox")]
	public class ComboBoxTests
	{
		#region Fields

		public static string ApplicationPath;

		#endregion

		#region Methods

		[TestMethod]
		public void Expand()
		{
		}

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

		#endregion
	}
}