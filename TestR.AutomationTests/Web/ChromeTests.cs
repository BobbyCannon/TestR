﻿#region References

using System;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.PowerShell;
using TestR.Web;
using TestR.Web.Browsers;

#endregion

namespace TestR.AutomationTests.Web
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "Chrome")]
	public class ChromeTests : TestCmdlet
	{
		#region Methods

		[TestMethod]
		public void Attach()
		{
			using (var browser = Chrome.Create())
			{
				Assert.IsNotNull(browser);
			}

			using (var browser = Chrome.Attach())
			{
				Assert.IsNotNull(browser);
				Console.WriteLine(browser.Id);
				browser.NavigateTo($"http://{Environment.MachineName}");
				browser.ExecuteScript("window.location.href").Dump();
			}
		}

		[TestMethod]
		public void AttachOrCreate()
		{
			using (var browser = Chrome.AttachOrCreate())
			{
				Assert.IsNotNull(browser);
				Console.WriteLine(browser.Id);
				browser.NavigateTo($"http://{Environment.MachineName}");
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

		[TestMethod]
		public void Create()
		{
			using (var browser = Chrome.Create())
			{
				Assert.IsNotNull(browser);
				Console.WriteLine(browser.Id);
				browser.NavigateTo($"http://{Environment.MachineName}");
				browser.ExecuteScript("window.location.href").Dump();
			}
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			Browser.CloseBrowsers();
		}

		[TestInitialize]
		public void TestInitialize()
		{
			Browser.CloseBrowsers();
		}

		#endregion
	}
}