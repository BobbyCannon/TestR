#region References

using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Native;
using KeyConverter = TestR.Native.KeyConverter;

#endregion

namespace TestR.UnitTests
{
	[TestClass]
	public class KeyboardTests
	{
		#region Methods

		[TestMethod]
		public void CapsLock()
		{
			var expected = "{CAPSLOCK}";
			Assert.AreEqual(expected, Keyboard.FormatTextForTypeText("\x10"));
		}

		[TestMethod]
		public void ConvertBackspace()
		{
			var expected = "{BS}";
			Assert.AreEqual(expected, Keyboard.FormatTextForTypeText("\b"));
			Assert.AreEqual(expected, Keyboard.FormatTextForTypeText("\x08"));
		}

		[TestMethod]
		public void ConvertBraces()
		{
			Assert.AreEqual("{{}", Keyboard.FormatTextForTypeText("{{}"));
			Assert.AreEqual("{}}", Keyboard.FormatTextForTypeText("{}}"));
			Assert.AreEqual("{{}{}}", Keyboard.FormatTextForTypeText("{}"));
		}

		[TestMethod]
		public void ConvertBreak()
		{
			var expected = "{BREAK}";
			Assert.AreEqual(expected, Keyboard.FormatTextForTypeText("\x13"));
		}

		[TestMethod]
		public void ExistingFormattingShouldRemain()
		{
			foreach (var code in Keyboard.Codes)
			{
				Assert.AreEqual(code, Keyboard.FormatTextForTypeText(code));
			}

			var expected = string.Join("", Keyboard.Codes);
			Assert.AreEqual(expected, Keyboard.FormatTextForTypeText(expected));
		}

		[TestMethod]
		public void KeyConversions()
		{
			TestKey(0x7A, Key.Z);
			TestKey(0x7B, Key.OemOpenBrackets);
			TestKey(0x7C, Key.OemPipe);
			TestKey(0x7D, Key.OemCloseBrackets);
			TestKey(0x7E, Key.OemTilde);
			TestKey(0x7F, Key.Delete);
		}

		private void TestKey(int ascii, Key key, bool isShiftPressed = false)
		{
			Assert.AreEqual(ascii, KeyConverter.KeyToAsciiValue(key, isShiftPressed));
			Assert.AreEqual(key, KeyConverter.AsciiToKeyValue(ascii));
		}

		#endregion
	}
}