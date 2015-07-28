#region References

using System;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop;
using TestR.Desktop.Elements;
using TestR.PowerShell;

#endregion

namespace TestR.IntegrationTests
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "Notepad")]
	public class NotepadTests : TestCmdlet
	{
		#region Fields

		public static string NotepadApplicationPath = "C:\\Windows\\Notepad.exe";

		#endregion

		#region Methods

		[TestMethod]
		public void AddTextToDocument()
		{
			using (var application = Application.AttachOrCreate(NotepadApplicationPath))
			{
				var window = application.Children.Windows.First();
				TestHelper.PrintChildren(window);
				var document = window.Children.Edits["15"];
				document.Text = "Hello World : Sub Collection";
			}
		}
		
		[TestMethod]
		public void ClickMenu()
		{
			using (var application = Application.AttachOrCreate(NotepadApplicationPath))
			{
				application.BringToFront();
				var window = application.Children.Windows.First();
				var menuBar = window.Children.MenuBars.First();
				TestHelper.PrintChildren(menuBar);

				var menu = menuBar.GetChild<MenuItem>("Untitled - NotepadApplicationFile");
				Assert.IsNotNull(menu);

				Console.WriteLine(menu.ExpandCollapseState);
				Thread.Sleep(500);
				menu.Click();
				Console.WriteLine(menu.ExpandCollapseState);
				Thread.Sleep(500);
				menu.Collapse();
				Console.WriteLine(menu.ExpandCollapseState);
				Thread.Sleep(500);
				menu.Expand();
				Console.WriteLine(menu.ExpandCollapseState);
			}
		}

		[TestMethod]
		public void AddTextToDocumentUsingFinder()
		{
			using (var application = Application.AttachOrCreate(NotepadApplicationPath))
			{
				var window = application.Children.Windows.First();
				var document = window.GetChild<Edit>("15");
				document.Text = "Hello World : GetChild";
				//window.Close();
			}
		}

		[TestMethod]
		public void AddTextToDocumentUsingIndexer()
		{
			using (var application = Application.AttachOrCreate(NotepadApplicationPath))
			{
				var window = application.Children.Windows.First();
				var document = (Edit) window["15"];
				document.Text = "Hello World : Indexer";
				//window.Close();
			}
		}

		[TestMethod]
		public void ApplicationAttachShouldSucceed()
		{
			Application.CloseAll(NotepadApplicationPath);

			using (var application1 = Application.Create(NotepadApplicationPath))
			{
				Assert.IsNotNull(application1);

				using (var application2 = Application.Attach(NotepadApplicationPath))
				{
					Assert.IsNotNull(application2);
					Assert.AreEqual(application1.Handle, application2.Handle);
				}
			}
		}

		[TestMethod]
		public void ApplicationBringToFrontShouldSucceed()
		{
			using (var application2 = Application.AttachOrCreate(NotepadApplicationPath))
			{
				Assert.IsNotNull(application2);
				application2.BringToFront();
				Assert.IsTrue(application2.IsInFront());
			}
		}

		[TestMethod]
		public void ApplicationCreateShouldSucceed()
		{
			Application.CloseAll(NotepadApplicationPath);

			using (var application = Application.Create(NotepadApplicationPath))
			{
				Assert.IsNotNull(application);
			}
		}

		[TestMethod]
		public void ApplicationListElements()
		{
			using (var application = Application.AttachOrCreate(NotepadApplicationPath))
			{
				foreach (var window in application.Children.Windows)
				{
					window.UpdateChildren();
				}
			}
		}

		#endregion
	}
}