#region References

using System.Drawing;
using System.IO;
using System.Management.Automation;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop.Elements;
using TestR.Desktop.Pattern;
using TestR.Native;
using TestR.PowerShell;

#endregion

namespace TestR.AutomationTests.Desktop.Elements
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "CheckBox")]
	public class CheckBoxTests : TestCmdlet
	{
		#region Fields

		public static string ApplicationPath;

		#endregion

		#region Methods

		[TestMethod]
		public void CheckByClickingForThreeStates()
		{
			Application.CloseAll(ApplicationPath);

			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.First<CheckBox>("checkBox3");
				checkbox.Click();
				Assert.AreEqual(ToggleState.Off, checkbox.CheckedState);
				checkbox.Click();
				Assert.AreEqual(ToggleState.On, checkbox.CheckedState);
				checkbox.Click();
				Assert.AreEqual(ToggleState.Indeterminate, checkbox.CheckedState);
			}
		}

		[TestMethod]
		public void CheckByClickingForTwoStates()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.First<CheckBox>("checkBox1");
				checkbox.Click();
				Assert.AreEqual(ToggleState.On, checkbox.CheckedState);
				checkbox.Click();
				Assert.AreEqual(ToggleState.Off, checkbox.CheckedState);
			}
		}

		[TestMethod]
		public void CheckByClickingWhileDisabled()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.First<CheckBox>("checkBox4");
				checkbox.Click();
				Assert.AreEqual(ToggleState.Off, checkbox.CheckedState);
			}
		}

		[TestMethod]
		public void CheckByTogglingForThreeStates()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.First<CheckBox>("checkBox3");
				checkbox.Toggle();
				Assert.AreEqual(ToggleState.Off, checkbox.CheckedState);
				checkbox.Toggle();
				Assert.AreEqual(ToggleState.On, checkbox.CheckedState);
				checkbox.Toggle();
				Assert.AreEqual(ToggleState.Indeterminate, checkbox.CheckedState);
			}
		}

		[TestMethod]
		public void CheckByTogglingForTwoStates()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.First<CheckBox>("checkBox1");
				checkbox.Toggle();
				Assert.AreEqual(ToggleState.On, checkbox.CheckedState);
				checkbox.Toggle();
				Assert.AreEqual(ToggleState.Off, checkbox.CheckedState);
			}
		}

		[TestMethod]
		public void CheckedShouldBeChecked()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.First<CheckBox>("checkBox2");
				Assert.AreEqual(true, checkbox.Checked);
				checkbox.Toggle();
				Assert.AreEqual(false, checkbox.Checked);
			}
		}

		[TestMethod]
		public void CheckedStateShouldBeChecked()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.First<CheckBox>("checkBox2");
				Assert.AreEqual(ToggleState.On, checkbox.CheckedState);
			}
		}

		[TestMethod]
		public void CheckedStateShouldBeIndeterminate()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.First<CheckBox>("checkBox3");
				Assert.AreEqual(ToggleState.Indeterminate, checkbox.CheckedState);
			}
		}

		[TestMethod]
		public void CheckedStateShouldBeUnchecked()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.First<CheckBox>("checkBox1");
				Assert.AreEqual(ToggleState.Off, checkbox.CheckedState);
			}
		}

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
		public void EnabledShouldBeFalse()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.First<CheckBox>("checkBox4");
				Assert.AreEqual(false, checkbox.Enabled);
			}
		}

		[TestMethod]
		public void EnabledShouldBeTrue()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.First<CheckBox>("checkBox1");
				Assert.AreEqual(true, checkbox.Enabled);
			}
		}

		[TestMethod]
		public void KeyboardFocusableShouldBeFalse()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.First<CheckBox>("checkBox4");
				Assert.AreEqual(false, checkbox.KeyboardFocusable);
			}
		}

		[TestMethod]
		public void KeyboardFocusableShouldBeTrue()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.First<CheckBox>("checkBox1");
				Assert.AreEqual(true, checkbox.KeyboardFocusable);
			}
		}

		[TestMethod]
		public void LocationShouldBeValid()
		{
			Application.CloseAll(ApplicationPath);
			Mouse.MoveTo(0, 0);

			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.First<CheckBox>("checkBox1");
				var x = application.Location.X + 248;
				var y = application.Location.Y + 173;
				Assert.AreEqual(new Point(x, y), checkbox.Location);
			}
		}

		[TestMethod]
		public void SizeShouldBeValid()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.First<CheckBox>("checkBox1");
				Assert.AreEqual(new Size(82, 17), checkbox.Size);
			}
		}

		[TestInitialize]
		public void TestInitialize()
		{
			Application.CloseAll(ApplicationPath);
		}

		[TestMethod]
		public void VisibleShouldBeFalse()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.FirstOrDefault<CheckBox>("checkBox5", wait: false);
				// Cannot be found because it's not visible.
				Assert.IsNull(checkbox);
			}
		}

		[TestMethod]
		public void VisibleShouldBeTrue()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.First<CheckBox>("checkBox1");
				Assert.AreEqual(true, checkbox.Visible);
			}
		}

		#endregion
	}
}