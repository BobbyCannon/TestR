#region References

using System;
using System.Collections.Generic;
using System.Threading;
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

		private NativeInput.KeyboardHookDelegate _keyboardCallback;
		private IntPtr _keyboardHandle;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Keyboard" /> class using an instance of a
		/// <see cref="InputMessageDispatcher" /> for dispatching <see cref="InputTypeWithData" /> messages.
		/// </summary>
		public Keyboard()
		{
			State = new KeyboardState();
		}

		#endregion

		#region Properties

		/// <summary>
		/// The last state of the keyboard when monitoring.
		/// </summary>
		public KeyboardState State { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Determines whether the specified key is up or down.
		/// </summary>
		/// <param name="key"> The <see cref="KeyboardKey" /> for the key. </param>
		/// <returns>
		/// True if the key is down otherwise false.
		/// </returns>
		/// <remarks>
		/// See: http://msdn.microsoft.com/en-us/library/ms646301(VS.85).aspx
		/// </remarks>
		public bool IsKeyDown(KeyboardKey key)
		{
			var result = NativeInput.GetKeyState((ushort) key);
			return result < 0;
		}

		/// <summary>
		/// Determines whether the specified key is up or down by calling the GetKeyState function.
		/// </summary>
		/// <param name="key"> The <see cref="KeyboardKey" /> for the key. </param>
		/// <returns>
		/// True if the key is up otherwise false.
		/// </returns>
		public bool IsKeyUp(KeyboardKey key)
		{
			return !IsKeyDown(key);
		}

		/// <summary>
		/// Determines whether the toggling key is toggled on (in-effect) or not by calling the GetKeyState function.
		/// </summary>
		/// <param name="key"> The <see cref="KeyboardKey" /> for the key. </param>
		/// <returns>
		/// True if the toggling key is toggled on (in-effect) otherwise false.
		/// </returns>
		public bool IsTogglingKeyInEffect(KeyboardKey key)
		{
			var result = NativeInput.GetKeyState((ushort) key);
			return (result & 0x01) == 0x01;
		}

		/// <summary>
		/// Calls the Input.SendInput method to simulate key down.
		/// </summary>
		/// <param name="keys"> The key(s) to press down. </param>
		public Keyboard KeyDown(params KeyboardKey[] keys)
		{
			Input.SendInput(new InputBuilder().AddKeyDown(keys));
			return this;
		}

		/// <summary>
		/// Simulates a key press for each of the specified key codes in the order they are specified.
		/// </summary>
		/// <param name="keys"> The keys to press. </param>
		public Keyboard KeyPress(params KeyboardKey[] keys)
		{
			Input.SendInput(new InputBuilder().AddKeyPress(keys));
			return this;
		}

		/// <summary>
		/// Simulates a modified keystroke where there is one modifier and multiple keys like CTRL-K-C where CTRL is the
		/// modifierKey and K and C are the keys.
		/// The flow is Modifier KeyDown, Keys Press in order, Modifier KeyUp.
		/// </summary>
		/// <param name="modifier"> The modifier key </param>
		/// <param name="keys"> The list of keys to simulate </param>
		public Keyboard KeyPress(KeyboardModifier modifier, params KeyboardKey[] keys)
		{
			Input.SendInput(new InputBuilder().AddKeyPress(modifier, keys));
			return this;
		}

		/// <summary>
		/// Simulates a modified keystroke where there are multiple modifiers and multiple keys like CTRL-ALT-K-C where CTRL
		/// and ALT are the modifierKeys and K and C are the keys.
		/// The flow is Modifiers KeyDown in order, Keys Press in order, Modifiers KeyUp in reverse order.
		/// </summary>
		/// <param name="modifiers"> The list of modifier keys </param>
		/// <param name="keys"> The list of keys to simulate </param>
		public Keyboard KeyPress(IEnumerable<KeyboardModifier> modifiers, params KeyboardKey[] keys)
		{
			var builder = new InputBuilder();
			builder.AddKeyPress(modifiers, keys);
			Input.SendInput(builder.ToArray());
			return this;
		}
		
		/// <summary>
		/// Calls the Input.SendInput method to simulate key up.
		/// </summary>
		/// <param name="keys"> The key(s) to lift up. </param>
		public Keyboard KeyUp(params KeyboardKey[] keys)
		{
			var inputList = new InputBuilder().AddKeyUp(keys).ToArray();
			Input.SendInput(inputList);
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
		/// Start monitoring the keyboard input.
		/// </summary>
		public void StartMonitoring()
		{
			const int lowLevelKeyboardHook = 13;
			_keyboardCallback = KeyboardHookCallback;
			_keyboardHandle = NativeInput.SetWindowsHookEx(lowLevelKeyboardHook, _keyboardCallback, IntPtr.Zero, 0);

			State.IsMonitoring = true;
		}

		/// <summary>
		/// Stop monitoring the keyboard input.
		/// </summary>
		public void StopMonitoring()
		{
			NativeInput.UnhookWindowsHookEx(_keyboardHandle);
			State.IsMonitoring = false;
		}

		/// <summary>
		/// Calls the Win32 SendInput method with a stream of KeyDown and KeyUp messages in order to simulate uninterrupted
		/// text entry via the keyboard.
		/// </summary>
		/// <param name="text"> The text to be simulated. </param>
		/// <param name="keys"> An optional set of keyboard keys to press after typing the provided text. </param>
		public Keyboard SendInput(string text, params KeyboardKey[] keys)
		{
			return SendInput(text, TimeSpan.Zero, keys);
		}
		
		/// <summary>
		/// Calls the Win32 SendInput method with a stream of KeyDown and KeyUp messages in order to simulate uninterrupted
		/// text entry via the keyboard.
		/// </summary>
		/// <param name="keys"> An optional set of keyboard keys to press after typing the provided text. </param>
		public Keyboard SendInput(params KeyboardKey[] keys)
		{
			return SendInput(string.Empty, keys);
		}
		
		/// <summary>
		/// Calls the Win32 SendInput method with a stream of KeyDown and KeyUp messages in order to simulate uninterrupted
		/// text entry via the keyboard.
		/// </summary>
		/// <param name="modifier"> The modifier key. </param>
		/// <param name="keys"> An optional set of keyboard keys to press after typing the provided text. </param>
		public Keyboard SendInput(KeyboardModifier modifier, params KeyboardKey[] keys)
		{
			Input.SendInput(new InputBuilder().AddKeyPress(modifier, keys));
			return this;
		}

		/// <summary>
		/// Calls the Win32 SendInput method with a stream of KeyDown and KeyUp messages in order to simulate uninterrupted
		/// text entry via the keyboard.
		/// </summary>
		/// <param name="text"> The text to be simulated. </param>
		/// <param name="delay"> An optional delay before sending optional keys. </param>
		/// <param name="strokes"> An optional set of keyboard strokes to process typing the provided text. </param>
		public Keyboard SendInput(string text, TimeSpan delay, params KeyStroke[] strokes)
		{
			if (text.Length > uint.MaxValue / 2)
			{
				throw new ArgumentException($"The text parameter is too long. It must be less than {uint.MaxValue / 2} characters.", nameof(text));
			}

			var inputList = new InputBuilder().AddCharacters(text);
			Input.SendInput(inputList.ToArray());

			if (delay > TimeSpan.Zero)
			{
				Thread.Sleep(delay);
			}

			Input.SendInput(inputList.Reset(strokes).ToArray());

			return this;
		}

		/// <summary>
		/// Calls the Win32 SendInput method with a stream of KeyDown and KeyUp messages in order to simulate uninterrupted
		/// text entry via the keyboard.
		/// </summary>
		/// <param name="text"> The text to be simulated. </param>
		/// <param name="delay"> An optional delay before sending optional keys. </param>
		/// <param name="keys"> An optional set of keyboard keys to press after typing the provided text. </param>
		public Keyboard SendInput(string text, TimeSpan delay, params KeyboardKey[] keys)
		{
			if (text.Length > uint.MaxValue / 2)
			{
				throw new ArgumentException($"The text parameter is too long. It must be less than {uint.MaxValue / 2} characters.", nameof(text));
			}

			var inputList = new InputBuilder()
				.AddCharacters(text)
				.ToArray();

			Input.SendInput(inputList);

			if (delay > TimeSpan.Zero)
			{
				Thread.Sleep(delay);
			}

			foreach (var key in keys)
			{
				Input.Keyboard.KeyPress(key);
			}

			return this;
		}

		/// <summary>
		/// Simulates a single character text entry via the keyboard.
		/// </summary>
		/// <param name="character"> The unicode character to be simulated. </param>
		public Keyboard SendInput(char character)
		{
			var inputList = new InputBuilder().AddCharacter(character).ToArray();
			Input.SendInput(inputList);
			return this;
		}

		private int KeyboardHookCallback(int code, int wParam, ref NativeInput.KeyboardHookStruct lParam)
		{
			const int wParamKeydown = 0x0100;

			if (code >= 0)
			{
				//Debug.WriteLine($"Code: {code}, wParam: {wParam}, lParam.vkCode: {lParam.vkCode}; flags: {lParam.flags}");

				var key = (KeyboardKey) lParam.vkCode;
				var isPressed = wParam == wParamKeydown;
				
				switch (key)
				{
					case KeyboardKey.CapsLock:
						State.IsCapsLockOn = IsTogglingKeyInEffect(KeyboardKey.CapsLock);
						break;
					
					case KeyboardKey.Alt:
						State.IsLeftAltPressed = isPressed;
						State.IsRightAltPressed = isPressed;
						break;
					
					case KeyboardKey.LeftAlt:
						State.IsLeftAltPressed = isPressed;
						break;
					
					case KeyboardKey.RightAlt:
						State.IsRightAltPressed = isPressed;
						break;
					
					case KeyboardKey.Control:
						State.IsLeftControlPressed = isPressed;
						State.IsRightControlPressed = isPressed;
						break;

					case KeyboardKey.LeftControl:
						State.IsLeftControlPressed = isPressed;
						break;
					
					case KeyboardKey.RightControl:
						State.IsRightControlPressed = isPressed;
						break;

					case KeyboardKey.Shift:
						State.IsLeftShiftPressed = isPressed;
						State.IsRightShiftPressed = isPressed;
						break;
					
					case KeyboardKey.LeftShift:
						State.IsLeftShiftPressed = isPressed;
						break;

					case KeyboardKey.RightShift:
						State.IsRightShiftPressed = isPressed;
						break;
				}

				State.Key = key;
				State.IsPressed = isPressed;
				
				// Debug.WriteLine(State.ToDebugString());

				KeyPressed?.Invoke(this, State.Clone());
			}

			return NativeInput.CallNextHookEx(_keyboardHandle, code, wParam, ref lParam);
		}

		#endregion

		#region Events

		/// <summary>
		/// Called when monitoring keyboard and a key is pressed.
		/// </summary>
		public event EventHandler<KeyboardState> KeyPressed;

		#endregion
	}
}