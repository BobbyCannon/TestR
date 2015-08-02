#region References

using System.IO;
using System.Management.Automation;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop;
using TestR.Desktop.Elements;
using TestR.Extensions;
using TestR.PowerShell;
using System.Drawing;

#endregion

namespace TestR.IntegrationTests.Desktop
{
	[TestClass]
	[Cmdlet(VerbsDiagnostic.Test, "CheckBoxes")]
	public class CheckBoxTests : TestCmdlet
	{
		#region Fields

		public static string ApplicationPath;

		#endregion

		#region Methods

		[TestMethod]
		public void CheckByClickingForThreeStates()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.GetChild<CheckBox>("checkBox3");
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
				var checkbox = application.GetChild<CheckBox>("checkBox1");
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
				var checkbox = application.GetChild<CheckBox>("checkBox4");
				checkbox.Click();
				Assert.AreEqual(ToggleState.Off, checkbox.CheckedState);
			}
		}

		[TestMethod]
		public void CheckByTogglingForThreeStates()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.GetChild<CheckBox>("checkBox3");
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
				var checkbox = application.GetChild<CheckBox>("checkBox1");
				checkbox.Toggle();
				Assert.AreEqual(ToggleState.On, checkbox.CheckedState);
				checkbox.Toggle();
				Assert.AreEqual(ToggleState.Off, checkbox.CheckedState);
			}
		}

		[TestMethod]
		public void CheckedStateShouldBeChecked()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.GetChild<CheckBox>("checkBox2");
				Assert.AreEqual(ToggleState.On, checkbox.CheckedState);
			}
		}

		[TestMethod]
		public void CheckedStateShouldBeIndeterminate()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.GetChild<CheckBox>("checkBox3");
				Assert.AreEqual(ToggleState.Indeterminate, checkbox.CheckedState);
			}
		}

		[TestMethod]
		public void CheckedStateShouldBeUnchecked()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.GetChild<CheckBox>("checkBox1");
				Assert.AreEqual(ToggleState.Off, checkbox.CheckedState);
			}
		}

		[TestMethod]
		public void EnabledShouldBeFalse()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.GetChild<CheckBox>("checkBox4");
				Assert.AreEqual(false, checkbox.Enabled);
			}
		}

		[TestMethod]
		public void EnabledShouldBeTrue()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.GetChild<CheckBox>("checkBox1");
				Assert.AreEqual(true, checkbox.Enabled);
			}
		}

		[TestMethod]
		public void KeyboardFocusableShouldBeTrue()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.GetChild<CheckBox>("checkBox1");
				Assert.AreEqual(true, checkbox.KeyboardFocusable);
			}
		}

		[TestMethod]
		public void KeyboardFocusableShouldBeFalse()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.GetChild<CheckBox>("checkBox4");
				Assert.AreEqual(false, checkbox.KeyboardFocusable);
			}
		}

		[TestMethod]
		public void LocationShouldBeValid()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.GetChild<CheckBox>("checkBox1");
				Assert.AreEqual(new Point(737,371), checkbox.Location);
			}
		}

		[TestMethod]
		public void SizeShouldBeValid()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.GetChild<CheckBox>("checkBox1");
				Assert.AreEqual(new Size(67,17), checkbox.Size);
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

		[TestMethod]
		public void TextShouldBeLabelValue()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.GetChild<CheckBox>("checkBox1");
				Assert.AreEqual("Unchecked", checkbox.Text);
			}
		}

		[TestMethod]
		public void VisibleShouldBeFalse()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.GetChild<CheckBox>("checkBox5");
				// Cannot be found because it's not visible.
				Assert.IsNull(checkbox);
			}
		}

		[TestMethod]
		public void VisibleShouldBeTrue()
		{
			using (var application = Application.AttachOrCreate(ApplicationPath))
			{
				var checkbox = application.GetChild<CheckBox>("checkBox1");
				Assert.AreEqual(true, checkbox.Visible);
			}
		}

		#endregion
	}
}