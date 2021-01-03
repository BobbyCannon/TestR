#region References

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop;
using TestR.Internal.Inputs;
using TestR.Internal.Native;

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
			Assert.IsFalse(Input.Keyboard.IsKeyDown(KeyboardKey.A));
			Assert.IsTrue(Input.Keyboard.IsKeyUp(KeyboardKey.A));

			StartNotepad();

			Input.Keyboard.KeyDown(KeyboardKey.A);

			Assert.IsTrue(Input.Keyboard.IsKeyDown(KeyboardKey.A));
			Assert.IsFalse(Input.Keyboard.IsKeyUp(KeyboardKey.A));

			Input.Keyboard.KeyUp(KeyboardKey.A);

			Assert.IsFalse(Input.Keyboard.IsKeyDown(KeyboardKey.A));
			Assert.IsTrue(Input.Keyboard.IsKeyUp(KeyboardKey.A));

			CloseNotepad();
		}

		[TestMethod]
		public void SetNotepadTextAndMove()
		{
			StartNotepad();

			Input.Keyboard
				.SendInput("These are your orders if you choose to accept them...")
				.KeyPress(KeyboardKey.Enter)
				.SendInput("This message will self destruct in 1 seconds.")
				.Sleep(1000)
				.KeyPress(KeyboardModifier.Alt, KeyboardKey.Space)
				.KeyPress(KeyboardKey.DownArrow)
				.KeyPress(KeyboardKey.Return);

			for (var i = 0; i < 5; i++)
			{
				Input.Keyboard.KeyPress(KeyboardKey.DownArrow).Sleep(100);
			}

			for (var i = 0; i < 5; i++)
			{
				Input.Keyboard.KeyPress(KeyboardKey.UpArrow).Sleep(100);
			}

			Input.Keyboard.KeyPress(KeyboardKey.Return);

			CloseNotepad();
		}

		[TestMethod]
		public void SetNotepadTextAndMoveInOneCommand()
		{
			StartNotepad();

			const int delay = 100;

			Input.Keyboard
				.SendInput("These are your orders if you choose to accept them...", KeyboardKey.Enter)
				.SendInput("This message will self destruct in 1 seconds.",
					TimeSpan.FromMilliseconds(1000),
					new KeyStroke(KeyboardModifier.Alt, KeyboardKey.Space),
					new KeyStroke(KeyboardKey.DownArrow),
					new KeyStroke(KeyboardKey.Return)
				)
				.KeyPress(KeyboardKey.DownArrow).Sleep(delay)
				.KeyPress(KeyboardKey.DownArrow).Sleep(delay)
				.KeyPress(KeyboardKey.DownArrow).Sleep(delay)
				.KeyPress(KeyboardKey.DownArrow).Sleep(delay)
				.KeyPress(KeyboardKey.DownArrow).Sleep(delay)
				.KeyPress(KeyboardKey.UpArrow).Sleep(delay)
				.KeyPress(KeyboardKey.UpArrow).Sleep(delay)
				.KeyPress(KeyboardKey.UpArrow).Sleep(delay)
				.KeyPress(KeyboardKey.UpArrow).Sleep(delay)
				.KeyPress(KeyboardKey.UpArrow).Sleep(delay)
				.KeyPress(KeyboardKey.Enter);

			CloseNotepad();
		}

		private void CloseNotepad()
		{
			Input.Keyboard
				.KeyPress(KeyboardModifier.Alt, KeyboardKey.F4)
				.KeyPress(KeyboardKey.N);
		}

		private static void StartNotepad()
		{
			Input.Keyboard
				.KeyPress(KeyboardKey.LeftWindows, KeyboardKey.R)
				.Sleep(250)
				//.SendInput("notepad")
				//.Sleep(250)
				//.KeyPress(KeyboardKey.Return)
				.Sleep(1000);
		}

		[TestMethod]
		public void name()
		{
			//StartNotepad();
			//var test = NativeInput.MapVirtualKey('f', 0);
			//((KeyboardKey) test).Dump();

			//var notepad = Application.Attach("c:\\windows\\system32\\notepad.exe");
			var application = Application.AttachOrCreate("C:\\Workspaces\\GitHub\\TestR\\TestR.TestWinForms\\bin\\Debug\\TestR.TestWinForms.exe");
			application.BringToFront();
			application.Focus();

			ushort character = (byte) 'f';
			character.Dump();

			var scanCode = (ushort) NativeInput.MapVirtualKey(character, 0);
			scanCode.Dump();

			var input = new InputTypeWithData
			{
				Type = (uint) InputType.Keyboard,
				Data =
				{
					Keyboard =
						new KeyboardInput
						{
							KeyCode = character,
							Scan = (ushort) NativeInput.MapVirtualKey(character, 0),
							Flags = (uint) KeyboardFlag.ScanCode,
							Time = 0,
							ExtraInfo = IntPtr.Zero
						}
				}
			};

			
			Input.SendInput(input);
			
			input.Data.Keyboard.Flags |= (uint) KeyboardFlag.KeyUp;

			Input.SendInput(input);
		}

		#endregion
	}
}