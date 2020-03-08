#region References

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop;

#endregion

namespace TestR.Tests.Desktop
{
	[TestClass]
	public class InputKeyboardTests
	{
		#region Methods

		[TestMethod]
		public void IsKeyDownAndIsKeyUp()
		{
			Assert.IsFalse(Input.Keyboard.IsKeyDown(KeyboardKeys.A));
			Assert.IsTrue(Input.Keyboard.IsKeyUp(KeyboardKeys.A));

			StartNotepad();

			Input.Keyboard.KeyDown(KeyboardKeys.A);

			Assert.IsTrue(Input.Keyboard.IsKeyDown(KeyboardKeys.A));
			Assert.IsFalse(Input.Keyboard.IsKeyUp(KeyboardKeys.A));

			Input.Keyboard.KeyUp(KeyboardKeys.A);

			Assert.IsFalse(Input.Keyboard.IsKeyDown(KeyboardKeys.A));
			Assert.IsTrue(Input.Keyboard.IsKeyUp(KeyboardKeys.A));

			CloseNotepad();
		}

		[TestMethod]
		public void SetNotepadTextAndMove()
		{
			StartNotepad();

			Input.Keyboard
				.TypeText("These are your orders if you choose to accept them...")
				.TypeText("This message will self destruct in 1 seconds.")
				.Sleep(1000)
				.ModifiedKeyStroke(KeyboardKeys.Alt, KeyboardKeys.Space)
				.KeyPress(KeyboardKeys.Down)
				.KeyPress(KeyboardKeys.Return);

			for (var i = 0; i < 5; i++)
			{
				Input.Keyboard.KeyPress(KeyboardKeys.Down).Sleep(100);
			}

			for (var i = 0; i < 5; i++)
			{
				Input.Keyboard.KeyPress(KeyboardKeys.Up).Sleep(100);
			}

			Input.Keyboard.KeyPress(KeyboardKeys.Return);

			CloseNotepad();
		}

		private void CloseNotepad()
		{
			Input.Keyboard
				.ModifiedKeyStroke(KeyboardKeys.Alt, KeyboardKeys.F4)
				.KeyPress(KeyboardKeys.N);
		}

		private static void StartNotepad()
		{
			Input.Keyboard
				.ModifiedKeyStroke(KeyboardKeys.LeftWindows, KeyboardKeys.R)
				.Sleep(250)
				.TypeText("notepad")
				.Sleep(250)
				.KeyPress(KeyboardKeys.Return)
				.Sleep(1000);
		}

		#endregion
	}
}