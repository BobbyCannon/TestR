#region References

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestR.Desktop;

#endregion

namespace TestR.Tests.Desktop
{
	[TestClass]
	public class KeyboardStateTests
	{
		#region Methods

		[TestMethod]
		public void CharactersToString()
		{
			Assert.AreEqual("a", new KeyboardState { Key = KeyboardKey.A }.ToString());
			Assert.AreEqual("b", new KeyboardState { Key = KeyboardKey.B }.ToString());
			Assert.AreEqual("c", new KeyboardState { Key = KeyboardKey.C }.ToString());
			Assert.AreEqual("d", new KeyboardState { Key = KeyboardKey.D }.ToString());
			
			Assert.AreEqual("A", new KeyboardState { Key = KeyboardKey.A, IsLeftShiftPressed = true }.ToString());
			Assert.AreEqual("A", new KeyboardState { Key = KeyboardKey.A, IsRightShiftPressed = true }.ToString());
			Assert.AreEqual("A", new KeyboardState { Key = KeyboardKey.A, IsCapsLockOn = true }.ToString());
			Assert.AreEqual("B", new KeyboardState { Key = KeyboardKey.B, IsLeftShiftPressed = true }.ToString());
			Assert.AreEqual("B", new KeyboardState { Key = KeyboardKey.B, IsRightShiftPressed = true }.ToString());
			Assert.AreEqual("B", new KeyboardState { Key = KeyboardKey.B, IsCapsLockOn = true }.ToString());
		}

		[TestMethod]
		public void NumbersToString()
		{
			Assert.AreEqual("0", new KeyboardState { Key = KeyboardKey.Number0 }.ToString());
			Assert.AreEqual("1", new KeyboardState { Key = KeyboardKey.Number1 }.ToString());
			Assert.AreEqual("2", new KeyboardState { Key = KeyboardKey.Number2 }.ToString());
			Assert.AreEqual("3", new KeyboardState { Key = KeyboardKey.Number3 }.ToString());
			Assert.AreEqual("4", new KeyboardState { Key = KeyboardKey.Number4 }.ToString());
			Assert.AreEqual("5", new KeyboardState { Key = KeyboardKey.Number5 }.ToString());
			Assert.AreEqual("6", new KeyboardState { Key = KeyboardKey.Number6 }.ToString());
			Assert.AreEqual("7", new KeyboardState { Key = KeyboardKey.Number7 }.ToString());
			Assert.AreEqual("8", new KeyboardState { Key = KeyboardKey.Number8 }.ToString());
			Assert.AreEqual("9", new KeyboardState { Key = KeyboardKey.Number9 }.ToString());
			
			Assert.AreEqual("0", new KeyboardState { Key = KeyboardKey.Numpad0 }.ToString());
			Assert.AreEqual("1", new KeyboardState { Key = KeyboardKey.Numpad1 }.ToString());
			Assert.AreEqual("2", new KeyboardState { Key = KeyboardKey.Numpad2 }.ToString());
			Assert.AreEqual("3", new KeyboardState { Key = KeyboardKey.Numpad3 }.ToString());
			Assert.AreEqual("4", new KeyboardState { Key = KeyboardKey.Numpad4 }.ToString());
			Assert.AreEqual("5", new KeyboardState { Key = KeyboardKey.Numpad5 }.ToString());
			Assert.AreEqual("6", new KeyboardState { Key = KeyboardKey.Numpad6 }.ToString());
			Assert.AreEqual("7", new KeyboardState { Key = KeyboardKey.Numpad7 }.ToString());
			Assert.AreEqual("8", new KeyboardState { Key = KeyboardKey.Numpad8 }.ToString());
			Assert.AreEqual("9", new KeyboardState { Key = KeyboardKey.Numpad9 }.ToString());
		}

		#endregion
	}
}