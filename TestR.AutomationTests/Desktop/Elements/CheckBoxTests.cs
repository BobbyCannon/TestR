#region References

using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop.Elements;
using TestR.Desktop.Pattern;
using TestR.Native;

#endregion

namespace TestR.AutomationTests.Desktop.Elements
{
	[TestClass]
	public class CheckBoxTests : BaseTest
	{
		#region Methods

		[TestMethod]
		public void CheckByClickingForThreeStates()
		{
			using (var application = GetApplication())
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
			using (var application = GetApplication())
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
			using (var application = GetApplication())
			{
				var checkbox = application.First<CheckBox>("checkBox4");
				checkbox.Click();
				Assert.AreEqual(ToggleState.Off, checkbox.CheckedState);
			}
		}

		[TestMethod]
		public void CheckByTogglingForThreeStates()
		{
			using (var application = GetApplication())
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
			using (var application = GetApplication())
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
			using (var application = GetApplication())
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
			using (var application = GetApplication())
			{
				var checkbox = application.First<CheckBox>("checkBox2");
				Assert.AreEqual(ToggleState.On, checkbox.CheckedState);
			}
		}

		[TestMethod]
		public void CheckedStateShouldBeIndeterminate()
		{
			using (var application = GetApplication())
			{
				var checkbox = application.First<CheckBox>("checkBox3");
				Assert.AreEqual(ToggleState.Indeterminate, checkbox.CheckedState);
			}
		}

		[TestMethod]
		public void CheckedStateShouldBeUnchecked()
		{
			using (var application = GetApplication())
			{
				var checkbox = application.First<CheckBox>("checkBox1");
				Assert.AreEqual(ToggleState.Off, checkbox.CheckedState);
			}
		}

		[TestMethod]
		public void EnabledShouldBeFalse()
		{
			using (var application = GetApplication())
			{
				var checkbox = application.First<CheckBox>("checkBox4");
				Assert.AreEqual(false, checkbox.Enabled);
			}
		}

		[TestMethod]
		public void EnabledShouldBeTrue()
		{
			using (var application = GetApplication())
			{
				var checkbox = application.First<CheckBox>("checkBox1");
				Assert.AreEqual(true, checkbox.Enabled);
			}
		}

		[TestMethod]
		public void KeyboardFocusableShouldBeFalse()
		{
			using (var application = GetApplication())
			{
				var checkbox = application.First<CheckBox>("checkBox4");
				Assert.AreEqual(false, checkbox.KeyboardFocusable);
			}
		}

		[TestMethod]
		public void KeyboardFocusableShouldBeTrue()
		{
			using (var application = GetApplication())
			{
				var checkbox = application.First<CheckBox>("checkBox1");
				Assert.AreEqual(true, checkbox.KeyboardFocusable);
			}
		}

		[TestMethod]
		public void LocationShouldBeValid()
		{
			Mouse.MoveTo(0, 0);

			using (var application = GetApplication())
			{
				application.MoveWindow(0, 0);
				var window = application.First<Window>();
				var checkbox = window.First<CheckBox>("checkBox1");
				var x = window.Location.X + 115;
				var y = window.Location.Y + 146;
				Assert.AreEqual(new Point(x, y), checkbox.Location);
			}
		}

		[TestMethod]
		public void SizeShouldBeValid()
		{
			using (var application = GetApplication())
			{
				var checkbox = application.First<CheckBox>("checkBox1");
				Assert.AreEqual(new Size(82, 17), checkbox.Size);
			}
		}

		[TestMethod]
		public void VisibleShouldBeFalse()
		{
			using (var application = GetApplication())
			{
				var checkbox = application.FirstOrDefault<CheckBox>("checkBox5", wait: false);
				// Cannot be found because it's not visible.
				Assert.IsNull(checkbox);
			}
		}

		[TestMethod]
		public void VisibleShouldBeTrue()
		{
			using (var application = GetApplication())
			{
				var checkbox = application.First<CheckBox>("checkBox1");
				Assert.AreEqual(true, checkbox.Visible);
			}
		}

		#endregion
	}
}