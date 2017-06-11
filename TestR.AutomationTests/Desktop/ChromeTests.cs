﻿#region References

using System;
using System.Diagnostics;
using System.Management.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.PowerShell;
using TestR.Web;
using TestR.Web.Browsers;

#endregion

namespace TestR.AutomationTests.Desktop
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "Chrome")]
	public class ChromeTests : TestCmdlet
	{
		#region Methods

		[TestMethod]
		public void Attach()
		{
			int browserId;

			using (var browser1 = Chrome.Create())
			{
				Assert.IsNotNull(browser1);
				browserId = browser1.Application.Process.Id;
			}

			using (var browser2 = Chrome.Attach())
			{
				Assert.IsNotNull(browser2);
				Assert.AreEqual(browserId, browser2.Application.Process.Id);
			}
		}

		[TestMethod]
		public void AttachOrCreate()
		{
			using (var browser = Chrome.AttachOrCreate())
			{
				Assert.IsNotNull(browser);
				Console.WriteLine(browser.Id);
				browser.NavigateTo("http://testr.local");
				browser.ExecuteScript("window.location.href").Dump();
			}
		}

		[TestMethod]
		public void AttachToBrowser()
		{
			int processId;

			using (var browser1 = Chrome.Create())
			{
				Assert.IsNotNull(browser1);
				processId = browser1.Application.Process.Id;
			}

			var process = Process.GetProcessById(processId);
			using (var browser2 = Browser.AttachToBrowser(process))
			{
				Assert.IsNotNull(browser2);
				Assert.AreEqual(typeof(Chrome), browser2.GetType());
			}
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			Browser.CloseBrowsers();
		}

		[TestMethod]
		public void CloseAll()
		{
			Assert.IsFalse(Application.Exists(Chrome.BrowserName));
		}

		[TestMethod]
		public void Create()
		{
			using (var browser = Chrome.Create())
			{
				Assert.IsNotNull(browser);
				var expected = "https://testr.local/";
				browser.NavigateTo(expected);
				browser.ExecuteScript("window.location.href").Dump();
				Assert.AreEqual(expected, browser.Uri);
				browser.MoveWindow(100, 110, 800, 600);
				Assert.AreEqual(100, browser.Window.Location.X);
				Assert.AreEqual(110, browser.Window.Location.Y);
				Assert.AreEqual(800, browser.Window.Size.Width);
				Assert.AreEqual(600, browser.Window.Size.Height);
			}
		}

		[TestMethod]
		public void CreateTwoInstances()
		{
			using (var browser = Chrome.Create())
			{
				using (var browser2 = Chrome.Create())
				{
					var expected = "https://testr.local/Forms.html";
					Assert.IsNotNull(browser);
					browser.NavigateTo(expected);
					Assert.AreEqual(expected, browser.Uri);
					browser.MoveWindow(100, 110, 800, 600);
					Assert.AreEqual(100, browser.Window.Location.X);
					Assert.AreEqual(110, browser.Window.Location.Y);
					Assert.AreEqual(800, browser.Window.Size.Width);
					Assert.AreEqual(600, browser.Window.Size.Height);
					browser.Resize(400, 300);
					Assert.AreEqual(400, browser.Window.Size.Width);
					Assert.AreEqual(300, browser.Window.Size.Height);
					Assert.AreEqual(400, browser.Application.Size.Width);
					Assert.AreEqual(300, browser.Application.Size.Height);

					var expected2 = "https://testr.local/Forms2.html";
					Assert.IsNotNull(browser2);
					browser2.NavigateTo(expected2);
					Assert.AreEqual(expected2, browser2.Uri);
					browser2.MoveWindow(900, 120, 600, 480);
					Assert.AreEqual(900, browser2.Window.Location.X);
					Assert.AreEqual(120, browser2.Window.Location.Y);
					Assert.AreEqual(600, browser2.Window.Size.Width);
					Assert.AreEqual(480, browser2.Window.Size.Height);
					browser2.Resize(500, 400);
					Assert.AreEqual(500, browser2.Window.Size.Width);
					Assert.AreEqual(400, browser2.Window.Size.Height);
					Assert.AreEqual(500, browser2.Application.Size.Width);
					Assert.AreEqual(400, browser2.Application.Size.Height);

					Assert.AreEqual(browser.Application.Process.Id, browser2.Application.Process.Id);
					Assert.AreNotEqual(browser.Window.Handle, browser2.Window.Handle);
				}
			}
		}

		[TestInitialize]
		public void TestInitialize()
		{
			Browser.CloseBrowsers();
		}

		#endregion
	}
}