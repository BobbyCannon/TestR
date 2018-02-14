#region References

using System;
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
		protected static string _applicationPathX86;

		#endregion

		#region Constructors

		public BaseTest()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var path = Path.GetDirectoryName(assembly.Location);
			var info = new DirectoryInfo(path ?? "/");

			_applicationPath = info.FullName.Replace("TestR.AutomationTests", "TestR.TestWinForms") + "\\TestR.TestWinForms.exe";
			_applicationPathX86 = info.FullName.Replace("TestR.AutomationTests", "TestR.TestWinForms") + "\\TestR.TestWinForms-x86.exe";
			Application.CloseAll(_applicationPath);
			Application.CloseAll(_applicationPathX86);
		}

		#endregion

		#region Methods

		public Application GetApplication(bool x86 = false)
		{
			var path = x86 ? _applicationPathX86 : _applicationPath;
			Application.CloseAll(path);
			var response = Application.AttachOrCreate(path);
			response.Timeout = TimeSpan.FromSeconds(5);
			return response;
		}

		[TestCleanup]
		public void TestCleanup()
		{
			Application.CloseAll(_applicationPath);
			Application.CloseAll(_applicationPathX86);
		}

		[TestInitialize]
		public void TestInitialize()
		{
			Application.CloseAll(_applicationPath);
			Application.CloseAll(_applicationPathX86);
		}

		#endregion
	}
}