#region References

using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop;
using TestR.Extensions;
using TestR.PowerShell;

#endregion

namespace TestR.IntegrationTests.Desktop.Elements
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "Window")]
	public class WindowTests : TestCmdlet
	{
		#region Fields

		public static string ApplicationPath;

		#endregion

		#region Methods

		[TestMethod]
		public void Move()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Children.Windows.FirstOrDefault();
				var random = new Random();
				Assert.IsNotNull(window);
				var x = random.Next(0, 200);
				var y = random.Next(0, 200);
				window.Move(x, y);
				Assert.AreEqual(x, window.Location.X);
				Assert.AreEqual(y, window.Location.Y);
			}
		}

		[TestMethod]
		public void Resize()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Children.Windows.FirstOrDefault();
				var random = new Random();
				Assert.IsNotNull(window);
				var width = random.Next(100, 300);
				var height = random.Next(100, 300);
				window.Resize(width, height);
				Assert.AreEqual(width, window.Size.Width);
				Assert.AreEqual(height, window.Size.Height);
			}
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