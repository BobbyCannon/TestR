#region References

using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.PowerShell;

#endregion

namespace TestR.AutomationTests.Desktop
{
	public class BaseTest : TestCmdlet
	{
		#region Fields

		protected static string _applicationPath;

		#endregion

		#region Constructors

		public BaseTest()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var path = Path.GetDirectoryName(assembly.Location);
			var info = new DirectoryInfo(path ?? "/");

			_applicationPath = info.FullName + "\\TestR.TestWinForms.exe";
			Application.CloseAll(_applicationPath);
		}

		#endregion

		#region Methods

		public Application GetApplication()
		{
			Application.CloseAll(_applicationPath);
			return Application.AttachOrCreate(_applicationPath);
		}

		[TestInitialize]
		public void TestInitialize()
		{
			Application.CloseAll(_applicationPath);
		}

		#endregion
	}
}