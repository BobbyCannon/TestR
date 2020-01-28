﻿#region References

using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Web;
using TestR.Web.Browsers;

#endregion

namespace TestR.AutomationTests.Desktop
{
	[TestClass]
	public class FirefoxTests
	{
		#region Methods

		[TestMethod]
		public void Attach()
		{
			using (var browser = Firefox.Create())
			{
				Assert.IsNotNull(browser);
			}

			using (var browser = Firefox.Attach())
			{
				Assert.IsNotNull(browser);
				Console.WriteLine(browser.Id);
				browser.NavigateTo("http://testr.local");
				browser.ExecuteScript("window.location.href").Dump();
			}
		}

		[TestMethod]
		public void AttachOrCreate()
		{
			using (var browser = Firefox.AttachOrCreate())
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

			using (var browser1 = Firefox.Create())
			{
				Assert.IsNotNull(browser1);
				processId = browser1.Application.Process.Id;
			}

			var process = Process.GetProcessById(processId);
			using (var browser2 = Browser.AttachToBrowser(process))
			{
				Assert.IsNotNull(browser2);
				Assert.AreEqual(typeof(Firefox), browser2.GetType());
			}
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			Browser.CloseBrowsers();
		}

		[TestMethod]
		public void Create()
		{
			using (var browser = Firefox.Create())
			{
				Assert.IsNotNull(browser);
				var expected = "https://testr.local/";
				browser.NavigateTo(expected);
				browser.ExecuteScript("window.location.href").Dump();
				Assert.AreEqual(expected, browser.Uri);
				browser.MoveWindow(100, 110, 800, 600);
				Assert.AreEqual(100, browser.Location.X);
				Assert.AreEqual(110, browser.Location.Y);
				Assert.AreEqual(800, browser.Size.Width);
				Assert.AreEqual(600, browser.Size.Height);
			}
		}

		[TestMethod]
		public void CreateTwoInstances()
		{
			using (var browser = Firefox.Create())
			{
				using (var browser2 = Firefox.Create())
				{
					var expected = "https://testr.local/Forms.html";
					Assert.IsNotNull(browser);
					browser.NavigateTo(expected);
					Assert.AreEqual(expected, browser.Uri);
					browser.MoveWindow(100, 110, 1024, 800);
					Assert.AreEqual(100, browser.Location.X);
					Assert.AreEqual(110, browser.Location.Y);
					Assert.AreEqual(1024, browser.Size.Width);
					Assert.AreEqual(800, browser.Size.Height);
					
					var expected2 = "https://testr.local/main.html";
					Assert.IsNotNull(browser2);
					browser2.NavigateTo(expected2);
					Assert.AreEqual(expected2, browser2.Uri);
					browser2.MoveWindow(900, 120, 800, 600);
					Assert.AreEqual(900, browser2.Location.X);
					Assert.AreEqual(120, browser2.Location.Y);
					Assert.AreEqual(800, browser2.Size.Width);
					Assert.AreEqual(600, browser2.Size.Height);

					browser.Resize(900, 700);
					Assert.AreEqual(900, browser.Size.Width);
					Assert.AreEqual(700, browser.Size.Height);

					browser2.Resize(700, 800);
					Assert.AreEqual(700, browser2.Size.Width);
					Assert.AreEqual(800, browser2.Size.Height);

					Assert.AreEqual(browser.Application.Process.Id, browser2.Application.Process.Id);
					//Assert.AreEqual(browser.Window.Handle, browser2.Window.Handle);
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