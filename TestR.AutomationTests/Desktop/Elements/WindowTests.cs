﻿#region References

using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop.Elements;
using TestR.PowerShell;

#endregion

namespace TestR.AutomationTests.Desktop.Elements
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "Window")]
	public class WindowTests : TestCmdlet
	{
		#region Fields

		public static string ApplicationPath;

		#endregion

		#region Methods

		[ClassCleanup]
		public static void ClassCleanup()
		{
			Application.CloseAll(ApplicationPath);
		}

		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var path = Path.GetDirectoryName(assembly.Location);
			var info = new DirectoryInfo(path ?? "/");

			ApplicationPath = info.Parent?.Parent?.Parent?.FullName;
			ApplicationPath += "\\TestR.TestWinForms\\Bin\\" + (assembly.IsAssemblyDebugBuild() ? "Debug" : "Release") + "\\TestR.TestWinForms.exe";
			Application.CloseAll(ApplicationPath);
		}

		[TestMethod]
		public void MoveChildWindow()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.First<Window>();
				var childWindow = window.First<Window>();
				childWindow.Move(0, 0);
				Assert.AreEqual(2, childWindow.Location.X);
				Assert.AreEqual(2, childWindow.Location.Y);

				childWindow.Move(10, 20);
				Assert.AreEqual(12, childWindow.Location.X);
				Assert.AreEqual(22, childWindow.Location.Y);

				childWindow.Move(100, 110);
				Assert.AreEqual(102, childWindow.Location.X);
				Assert.AreEqual(112, childWindow.Location.Y);
			}
		}

		[TestMethod]
		public void MoveParentWindow()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.First<Window>();
				Assert.IsNotNull(window);

				var random = new Random();
				var x = random.Next(0, 200);
				var y = random.Next(0, 200);

				window.Move(x, y);
				Assert.AreEqual(x, window.Location.X);
				Assert.AreEqual(y, window.Location.Y);
			}
		}

		[TestMethod]
		public void ResizeChildWindow()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.First<Window>();
				var childWindow = window.First<Window>();

				childWindow.Resize(300, 200);
				Assert.AreEqual(300, childWindow.Width);
				Assert.AreEqual(200, childWindow.Height);

				childWindow.Resize(400, 300);
				Assert.AreEqual(400, childWindow.Width);
				Assert.AreEqual(300, childWindow.Height);
			}
		}

		[TestMethod]
		public void ResizeParentWindow()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var window = application.Descendants<Window>().FirstOrDefault();
				var random = new Random();
				Assert.IsNotNull(window);
				var width = random.Next(300, 600);
				var height = random.Next(300, 600);
				window.Resize(width, height);
				Assert.AreEqual(width, window.Size.Width);
				Assert.AreEqual(height, window.Size.Height);
			}
		}

		[TestInitialize]
		public void TestInitialize()
		{
			Application.CloseAll(ApplicationPath);
		}

		#endregion
	}
}