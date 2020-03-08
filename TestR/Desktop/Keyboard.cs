#region References

using System;
using System.Collections.Generic;
using System.Threading;
using TestR.Internal;
using TestR.Internal.Inputs;
using TestR.Internal.Native;

#endregion

namespace TestR.Desktop
{
	/// <summary>
	/// Represents the keyboard and allows for simulated input.
	/// </summary>
	public class Keyboard
	{
		#region Fields

		/// <summary>
		/// The instance of the <see cref="InputMessageDispatcher" /> to use for dispatching <see cref="Internal.Inputs.Input" /> messages.
		/// </summary>
		private readonly InputMessageDispatcher _messageDispatcher;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Keyboard" /> class using an instance of a <see cref="InputMessageDispatcher" /> for dispatching <see cref="Internal.Inputs.Input" /> messages.
		/// </summary>
		public Keyboard()
		{
			_messageDispatcher = new InputMessageDispatcher();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Determines whether the physical key is up or down at the time the function is called regardless of whether the application thread has read the keyboard event from the message pump by calling the <see cref="NativeInput.GetAsyncKeyState" /> function.
		/// </summary>
		/// <param name="keyCode"> The <see cref="KeyboardKeys" /> for the key. </param>
		/// <returns>
		/// <c> true </c> if the key is down; otherwise, <c> false </c>.
		/// </returns>
		/// <remarks>
		/// See: http://msdn.microsoft.com/en-us/library/ms646293(VS.85).aspx
		/// </remarks>
		public bool IsHardwareKeyDown(KeyboardKeys keyCode)
		{
			var result = NativeInput.GetAsyncKeyState((ushort) keyCode);
			return result < 0;
		}

		/// <summary>
		/// Determines whether the physical key is up or down at the time the function is called regardless of whether the application thread has read the keyboard event from the message pump by calling the <see cref="NativeInput.GetAsyncKeyState" /> function.
		/// </summary>
		/// <param name="keyCode"> The <see cref="KeyboardKeys" /> for the key. </param>
		/// <returns>
		/// <c> true </c> if the key is up; otherwise, <c> false </c>.
		/// </returns>
		/// <remarks>
		/// See: http://msdn.microsoft.com/en-us/library/ms646293(VS.85).aspx
		/// </remarks>
		public bool IsHardwareKeyUp(KeyboardKeys keyCode)
		{
			return !IsHardwareKeyDown(keyCode);
		}

		/// <summary>
		/// Determines whether the specified key is up or down by calling the GetKeyState function.
		/// </summary>
		/// <param name="keyCode"> The <see cref="KeyboardKeys" /> for the key. </param>
		/// <returns>
		/// <c> true </c> if the key is down; otherwise, <c> false </c>.
		/// </returns>
		/// <remarks>
		/// See: http://msdn.microsoft.com/en-us/library/ms646301(VS.85).aspx
		/// </remarks>
		public bool IsKeyDown(KeyboardKeys keyCode)
		{
			var result = NativeInput.GetKeyState((ushort) keyCode);
			return result < 0;
		}

		/// <summary>
		/// Determines whether the specified key is up or down by calling the GetKeyState function.
		/// </summary>
		/// <param name="keyCode"> The <see cref="KeyboardKeys" /> for the key. </param>
		/// <returns>
		/// <c> true </c> if the key is up; otherwise, <c> false </c>.
		/// </returns>
		/// <remarks>
		/// See: http://msdn.microsoft.com/en-us/library/ms646301(VS.85).aspx
		/// </remarks>
		public bool IsKeyUp(KeyboardKeys keyCode)
		{
			return !IsKeyDown(keyCode);
		}

		/// <summary>
		/// Determines whether the toggling key is toggled on (in-effect) or not by calling the GetKeyState function.
		/// </summary>
		/// <param name="keyCode"> The <see cref="KeyboardKeys" /> for the key. </param>
		/// <returns>
		/// <c> true </c> if the toggling key is toggled on (in-effect); otherwise, <c> false </c>.
		/// </returns>
		/// <remarks>
		/// See: http://msdn.microsoft.com/en-us/library/ms646301(VS.85).aspx
		/// </remarks>
		public bool IsTogglingKeyInEffect(KeyboardKeys keyCode)
		{
			var result = NativeInput.GetKeyState((ushort) keyCode);
			return (result & 0x01) == 0x01;
		}

		/// <summary>
		/// Calls the Win32 SendInput method to simulate a KeyDown.
		/// </summary>
		/// <param name="keyCode"> The <see cref="KeyboardKeys" /> to press </param>
		public Keyboard KeyDown(KeyboardKeys keyCode)
		{
			var inputList = new InputBuilder().AddKeyDown(keyCode).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Calls the Win32 SendInput method with a KeyDown and KeyUp message in the same input sequence in order to simulate a Key PRESS.
		/// </summary>
		/// <param name="keyCode"> The <see cref="KeyboardKeys" /> to press </param>
		public Keyboard KeyPress(KeyboardKeys keyCode)
		{
			var inputList = new InputBuilder().AddKeyPress(keyCode).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a key press for each of the specified key codes in the order they are specified.
		/// </summary>
		/// <param name="keyCodes"> </param>
		public Keyboard KeyPress(params KeyboardKeys[] keyCodes)
		{
			var builder = new InputBuilder();
			KeysPress(builder, keyCodes);
			SendSimulatedInput(builder.ToArray());
			return this;
		}

		/// <summary>
		/// Calls the Win32 SendInput method to simulate a KeyUp.
		/// </summary>
		/// <param name="keyCode"> The <see cref="KeyboardKeys" /> to lift up. </param>
		public Keyboard KeyUp(KeyboardKeys keyCode)
		{
			var inputList = new InputBuilder().AddKeyUp(keyCode).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a simple modified keystroke like CTRL-C where CTRL is the modifierKey and C is the key.
		/// The flow is Modifier KeyDown, Key Press, Modifier KeyUp.
		/// </summary>
		/// <param name="modifierKeyCode"> The modifier key. </param>
		/// <param name="keyCode"> The key to simulate. </param>
		public Keyboard ModifiedKeyStroke(KeyboardKeys modifierKeyCode, KeyboardKeys keyCode)
		{
			ModifiedKeyStroke(new[] { modifierKeyCode }, new[] { keyCode });
			return this;
		}

		/// <summary>
		/// Simulates a modified keystroke where there are multiple modifiers and one key like CTRL-ALT-C where CTRL and ALT are the modifierKeys and C is the key.
		/// The flow is Modifiers KeyDown in order, Key Press, Modifiers KeyUp in reverse order.
		/// </summary>
		/// <param name="modifierKeyCodes"> The list of modifier keys </param>
		/// <param name="keyCode"> The key to simulate </param>
		public Keyboard ModifiedKeyStroke(IEnumerable<KeyboardKeys> modifierKeyCodes, KeyboardKeys keyCode)
		{
			ModifiedKeyStroke(modifierKeyCodes, new[] { keyCode });
			return this;
		}

		/// <summary>
		/// Simulates a modified keystroke where there is one modifier and multiple keys like CTRL-K-C where CTRL is the modifierKey and K and C are the keys.
		/// The flow is Modifier KeyDown, Keys Press in order, Modifier KeyUp.
		/// </summary>
		/// <param name="modifierKey"> The modifier key </param>
		/// <param name="keyCodes"> The list of keys to simulate </param>
		public Keyboard ModifiedKeyStroke(KeyboardKeys modifierKey, IEnumerable<KeyboardKeys> keyCodes)
		{
			ModifiedKeyStroke(new[] { modifierKey }, keyCodes);
			return this;
		}

		/// <summary>
		/// Simulates a modified keystroke where there are multiple modifiers and multiple keys like CTRL-ALT-K-C where CTRL and ALT are the modifierKeys and K and C are the keys.
		/// The flow is Modifiers KeyDown in order, Keys Press in order, Modifiers KeyUp in reverse order.
		/// </summary>
		/// <param name="modifierKeyCodes"> The list of modifier keys </param>
		/// <param name="keyCodes"> The list of keys to simulate </param>
		public Keyboard ModifiedKeyStroke(IEnumerable<KeyboardKeys> modifierKeyCodes, IEnumerable<KeyboardKeys> keyCodes)
		{
			var builder = new InputBuilder();
			ModifiersDown(builder, modifierKeyCodes);
			KeysPress(builder, keyCodes);
			ModifiersUp(builder, modifierKeyCodes);

			SendSimulatedInput(builder.ToArray());
			return this;
		}

		/// <summary>
		/// Sleeps the executing thread to create a pause between simulated inputs.
		/// </summary>
		/// <param name="timeoutInMilliseconds"> The number of milliseconds to wait. </param>
		public Keyboard Sleep(int timeoutInMilliseconds)
		{
			Thread.Sleep(timeoutInMilliseconds);
			return this;
		}

		/// <summary>
		/// Sleeps the executing thread to create a pause between simulated inputs.
		/// </summary>
		/// <param name="timeout"> The time to wait. </param>
		public Keyboard Sleep(TimeSpan timeout)
		{
			Thread.Sleep(timeout);
			return this;
		}

		/// <summary>
		/// Calls the Win32 SendInput method with a stream of KeyDown and KeyUp messages in order to simulate uninterrupted text entry via the keyboard.
		/// </summary>
		/// <param name="text"> The text to be simulated. </param>
		public Keyboard TypeText(string text)
		{
			if (text.Length > uint.MaxValue / 2)
			{
				throw new ArgumentException($"The text parameter is too long. It must be less than {uint.MaxValue / 2} characters.", nameof(text));
			}
			var inputList = new InputBuilder().AddCharacters(text).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		/// <summary>
		/// Simulates a single character text entry via the keyboard.
		/// </summary>
		/// <param name="character"> The unicode character to be simulated. </param>
		public Keyboard TypeText(char character)
		{
			var inputList = new InputBuilder().AddCharacter(character).ToArray();
			SendSimulatedInput(inputList);
			return this;
		}

		private void KeysPress(InputBuilder builder, IEnumerable<KeyboardKeys> keyCodes)
		{
			if (keyCodes == null)
			{
				return;
			}

			foreach (var key in keyCodes)
			{
				builder.AddKeyPress(key);
			}
		}

		private void ModifiersDown(InputBuilder builder, IEnumerable<KeyboardKeys> modifierKeyCodes)
		{
			if (modifierKeyCodes == null)
			{
				return;
			}

			foreach (var key in modifierKeyCodes)
			{
				builder.AddKeyDown(key);
			}
		}

		private void ModifiersUp(InputBuilder builder, IEnumerable<KeyboardKeys> modifierKeyCodes)
		{
			if (modifierKeyCodes == null)
			{
				return;
			}

			// Key up in reverse
			var stack = new Stack<KeyboardKeys>(modifierKeyCodes);

			while (stack.Count > 0)
			{
				builder.AddKeyUp(stack.Pop());
			}
		}

		/// <summary>
		/// Sends the list of <see cref="Internal.Inputs.Input" /> messages using the <see cref="InputMessageDispatcher" /> instance.
		/// </summary>
		/// <param name="inputList"> The <see cref="System.Array" /> of <see cref="Internal.Inputs.Input" /> messages to send. </param>
		private void SendSimulatedInput(Internal.Inputs.Input[] inputList)
		{
			_messageDispatcher.DispatchInput(inputList);
		}

		#endregion
	}
}