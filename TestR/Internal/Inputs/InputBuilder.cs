#region References

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TestR.Desktop;
using TestR.Internal.Native;

#endregion

namespace TestR.Internal.Inputs
{
	/// <summary>
	/// A helper class for building a list of <see cref="Input" /> messages ready to be sent to the native Windows API.
	/// </summary>
	internal class InputBuilder : IEnumerable<Input>
	{
		#region Fields

		/// <summary>
		/// The public list of <see cref="Input" /> messages being built by this instance.
		/// </summary>
		private readonly List<Input> _inputList;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="InputBuilder" /> class.
		/// </summary>
		public InputBuilder()
		{
			_inputList = new List<Input>();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the <see cref="Input" /> at the specified position.
		/// </summary>
		/// <value> The <see cref="Input" /> message at the specified position. </value>
		public Input this[int position] => _inputList[position];

		#endregion

		#region Methods

		/// <summary>
		/// Move the mouse to an absolute position.
		/// </summary>
		/// <param name="absoluteX"> </param>
		/// <param name="absoluteY"> </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddAbsoluteMouseMovement(int absoluteX, int absoluteY)
		{
			var screen = Screen.FromPoint(absoluteX, absoluteY);
			var relativeX = absoluteX * 65536 / screen.Size.Width + 1;
			var relativeY = absoluteY * 65536 / screen.Size.Height + 1;

			var movement = new Input { Type = (uint) InputType.Mouse };
			movement.Data.Mouse.Flags = (uint) (MouseFlag.Move | MouseFlag.Absolute);
			movement.Data.Mouse.X = relativeX;
			movement.Data.Mouse.Y = relativeY;

			_inputList.Add(movement);

			return this;
		}

		/// <summary>
		/// Move the mouse to the absolute position on the virtual desktop.
		/// </summary>
		/// <param name="absoluteX"> </param>
		/// <param name="absoluteY"> </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddAbsoluteMouseMovementOnVirtualDesktop(int absoluteX, int absoluteY)
		{
			var screen = Screen.VirtualScreenSize;
			var relativeX = absoluteX * 65536 / screen.Size.Width + 1;
			var relativeY = absoluteY * 65536 / screen.Size.Height + 1;

			var movement = new Input { Type = (uint) InputType.Mouse };
			movement.Data.Mouse.Flags = (uint) (MouseFlag.Move | MouseFlag.Absolute | MouseFlag.VirtualDesk);
			movement.Data.Mouse.X = relativeX;
			movement.Data.Mouse.Y = relativeY;

			_inputList.Add(movement);

			return this;
		}

		/// <summary>
		/// Adds the character to the list of <see cref="Input" /> messages.
		/// </summary>
		/// <param name="character"> The <see cref="System.Char" /> to be added to the list of <see cref="Input" /> messages. </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddCharacter(char character)
		{
			ushort scanCode = character;

			var down = new Input
			{
				Type = (uint) InputType.Keyboard,
				Data =
				{
					Keyboard =
						new KeyboardInput
						{
							KeyCode = 0,
							Scan = scanCode,
							Flags = (uint) KeyboardFlag.Unicode,
							Time = 0,
							ExtraInfo = IntPtr.Zero
						}
				}
			};

			var up = new Input
			{
				Type = (uint) InputType.Keyboard,
				Data =
				{
					Keyboard =
						new KeyboardInput
						{
							KeyCode = 0,
							Scan = scanCode,
							Flags = (uint) (KeyboardFlag.KeyUp | KeyboardFlag.Unicode),
							Time = 0,
							ExtraInfo = IntPtr.Zero
						}
				}
			};

			// Handle extended keys:
			// If the scan code is preceded by a prefix byte that has the value 0xE0 (224),
			// we need to include the KEYEVENTF_EXTENDEDKEY flag in the Flags property. 
			if ((scanCode & 0xFF00) == 0xE000)
			{
				down.Data.Keyboard.Flags |= (uint) KeyboardFlag.ExtendedKey;
				up.Data.Keyboard.Flags |= (uint) KeyboardFlag.ExtendedKey;
			}

			_inputList.Add(down);
			_inputList.Add(up);
			return this;
		}

		/// <summary>
		/// Adds all of the characters in the specified <see cref="IEnumerable{T}" /> of <see cref="char" />.
		/// </summary>
		/// <param name="characters"> The characters to add. </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddCharacters(IEnumerable<char> characters)
		{
			characters.ForEach(x => AddCharacter(x));
			return this;
		}

		/// <summary>
		/// Adds the characters in the specified <see cref="string" />.
		/// </summary>
		/// <param name="characters"> The string of <see cref="char" /> to add. </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddCharacters(string characters)
		{
			return AddCharacters(characters.ToCharArray());
		}

		/// <summary>
		/// Adds a key down to the list of <see cref="Input" /> messages.
		/// </summary>
		/// <param name="keyCode"> The <see cref="KeyboardKey" />. </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddKeyDown(KeyboardKey keyCode)
		{
			var down =
				new Input
				{
					Type = (uint) InputType.Keyboard,
					Data =
					{
						Keyboard =
							new KeyboardInput
							{
								KeyCode = (ushort) keyCode,
								Scan = (ushort) (NativeInput.MapVirtualKey((uint) keyCode, 0) & 0xFFU),
								Flags = IsExtendedKey(keyCode) ? (uint) KeyboardFlag.ExtendedKey : 0,
								Time = 0,
								ExtraInfo = IntPtr.Zero
							}
					}
				};

			_inputList.Add(down);
			return this;
		}

		/// <summary>
		/// Adds a key down to the list of <see cref="Input" /> messages.
		/// </summary>
		/// <param name="keys"> The keys to press down. </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddKeyDown(params KeyboardKey[] keys)
		{
			keys.ForEach(x => AddKeyDown(x));
			return this;
		}

		/// <summary>
		/// Adds a key press to the list of <see cref="Input" /> messages which is equivalent to a key down followed by a key up.
		/// </summary>
		/// <param name="key"> The key to press down. </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddKeyPress(KeyboardKey key)
		{
			AddKeyDown(key);
			AddKeyUp(key);
			return this;
		}

		/// <summary>
		/// Adds a key press to the list of <see cref="Input" /> messages which is equivalent to a key down followed by a key up.
		/// </summary>
		/// <param name="keys"> The keys to press down. </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddKeyPress(params KeyboardKey[] keys)
		{
			keys.ForEach(x => AddKeyPress(x));
			return this;
		}

		/// <summary>
		/// Adds a key up to the list of <see cref="Input" /> messages.
		/// </summary>
		/// <param name="key"> The key to release. </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddKeyUp(KeyboardKey key)
		{
			var up =
				new Input
				{
					Type = (uint) InputType.Keyboard,
					Data =
					{
						Keyboard =
							new KeyboardInput
							{
								KeyCode = (ushort) key,
								Scan = (ushort) (NativeInput.MapVirtualKey((uint) key, 0) & 0xFFU),
								Flags = (uint) (IsExtendedKey(key)
									? KeyboardFlag.KeyUp | KeyboardFlag.ExtendedKey
									: KeyboardFlag.KeyUp),
								Time = 0,
								ExtraInfo = IntPtr.Zero
							}
					}
				};

			_inputList.Add(up);
			return this;
		}

		/// <summary>
		/// Adds a key up to the list of <see cref="Input" /> messages.
		/// </summary>
		/// <param name="keys"> The keys to release. </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddKeyUp(params KeyboardKey[] keys)
		{
			keys.ForEach(x => AddKeyUp(x));
			return this;
		}

		/// <summary>
		/// Adds a single click of the specified button.
		/// </summary>
		/// <param name="button"> </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddMouseButtonClick(MouseButton button)
		{
			return AddMouseButtonDown(button).AddMouseButtonUp(button);
		}

		/// <summary>
		/// Adds a double click of the specified button.
		/// </summary>
		/// <param name="button"> </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddMouseButtonDoubleClick(MouseButton button)
		{
			return AddMouseButtonClick(button).AddMouseButtonClick(button);
		}

		/// <summary>
		/// Adds a mouse button down for the specified button.
		/// </summary>
		/// <param name="button"> </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddMouseButtonDown(MouseButton button)
		{
			var buttonDown = new Input { Type = (uint) InputType.Mouse };
			buttonDown.Data.Mouse.Flags = (uint) ToMouseButtonDownFlag(button);

			_inputList.Add(buttonDown);

			return this;
		}

		/// <summary>
		/// Adds a mouse button up for the specified button.
		/// </summary>
		/// <param name="button"> </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddMouseButtonUp(MouseButton button)
		{
			var buttonUp = new Input { Type = (uint) InputType.Mouse };
			buttonUp.Data.Mouse.Flags = (uint) ToMouseButtonUpFlag(button);
			_inputList.Add(buttonUp);

			return this;
		}

		/// <summary>
		/// Scroll the horizontal mouse wheel by the specified amount.
		/// </summary>
		/// <param name="scrollAmount"> </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddMouseHorizontalWheelScroll(int scrollAmount)
		{
			var scroll = new Input { Type = (uint) InputType.Mouse };
			scroll.Data.Mouse.Flags = (uint) MouseFlag.HorizontalWheel;
			scroll.Data.Mouse.MouseData = (uint) scrollAmount;

			_inputList.Add(scroll);

			return this;
		}

		/// <summary>
		/// Scroll the vertical mouse wheel by the specified amount.
		/// </summary>
		/// <param name="scrollAmount"> </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddMouseVerticalWheelScroll(int scrollAmount)
		{
			var scroll = new Input { Type = (uint) InputType.Mouse };
			scroll.Data.Mouse.Flags = (uint) MouseFlag.VerticalWheel;
			scroll.Data.Mouse.MouseData = (uint) scrollAmount;

			_inputList.Add(scroll);

			return this;
		}

		/// <summary>
		/// Adds a single click of the specified button.
		/// </summary>
		/// <param name="xButtonId"> </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddMouseXButtonClick(int xButtonId)
		{
			return AddMouseXButtonDown(xButtonId).AddMouseXButtonUp(xButtonId);
		}

		/// <summary>
		/// Adds a double click of the specified button.
		/// </summary>
		/// <param name="xButtonId"> </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddMouseXButtonDoubleClick(int xButtonId)
		{
			return AddMouseXButtonClick(xButtonId).AddMouseXButtonClick(xButtonId);
		}

		/// <summary>
		/// Adds a mouse button down for the specified button.
		/// </summary>
		/// <param name="xButtonId"> </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddMouseXButtonDown(int xButtonId)
		{
			var buttonDown = new Input { Type = (uint) InputType.Mouse };
			buttonDown.Data.Mouse.Flags = (uint) MouseFlag.XDown;
			buttonDown.Data.Mouse.MouseData = (uint) xButtonId;
			_inputList.Add(buttonDown);

			return this;
		}

		/// <summary>
		/// Adds a mouse button up for the specified button.
		/// </summary>
		/// <param name="xButtonId"> </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddMouseXButtonUp(int xButtonId)
		{
			var buttonUp = new Input { Type = (uint) InputType.Mouse };
			buttonUp.Data.Mouse.Flags = (uint) MouseFlag.XUp;
			buttonUp.Data.Mouse.MouseData = (uint) xButtonId;
			_inputList.Add(buttonUp);

			return this;
		}

		/// <summary>
		/// Moves the mouse relative to its current position.
		/// </summary>
		/// <param name="x"> </param>
		/// <param name="y"> </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddRelativeMouseMovement(int x, int y)
		{
			var movement = new Input { Type = (uint) InputType.Mouse };
			movement.Data.Mouse.Flags = (uint) MouseFlag.Move;
			movement.Data.Mouse.X = x;
			movement.Data.Mouse.Y = y;

			_inputList.Add(movement);

			return this;
		}

		/// <summary>
		/// Add key strokes to input builder.
		/// </summary>
		/// <param name="stroke"> </param>
		public void AddStroke(KeyStroke stroke)
		{
			var modifierKeys = ConvertModifierToKey(stroke.Modifier).ToList();

			foreach (var key in modifierKeys)
			{
				AddKeyDown(key);
			}

			switch (stroke.Action)
			{
				case KeyboardAction.KeyDown:
					AddKeyDown(stroke.Key);
					break;

				case KeyboardAction.KeyUp:
					AddKeyUp(stroke.Key);
					break;

				case KeyboardAction.KeyPress:
				default:
					AddKeyPress(stroke.Key);
					break;
			}

			foreach (var key in modifierKeys)
			{
				AddKeyUp(key);
			}
		}

		public void AddStrokes(params KeyStroke[] strokes)
		{
			foreach (var stroke in strokes)
			{
				AddStroke(stroke);
			}
		}

		/// <summary>
		/// Clear the input list.
		/// </summary>
		public void Clear()
		{
			_inputList.Clear();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the list of <see cref="Input" /> messages.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the list of <see cref="Input" /> messages.
		/// </returns>
		/// <filterpriority> 1 </filterpriority>
		public IEnumerator<Input> GetEnumerator()
		{
			return _inputList.GetEnumerator();
		}

		/// <summary>
		/// Determines if the <see cref="KeyboardKey" /> is an ExtendedKey
		/// </summary>
		/// <param name="keyCode"> The key code. </param>
		/// <returns> true if the key code is an extended key; otherwise, false. </returns>
		/// <remarks>
		/// The extended keys consist of the ALT and CTRL keys on the right-hand side of the keyboard; the INS, DEL, HOME, END, PAGE UP, PAGE DOWN, and arrow keys in the clusters to the left of the numeric keypad; the NUM LOCK key; the BREAK (CTRL+PAUSE) key; the PRINT SCRN key; and the divide (/) and ENTER keys in the numeric keypad.
		/// See http://msdn.microsoft.com/en-us/library/ms646267(v=vs.85).aspx Section "Extended-Key Flag"
		/// </remarks>
		public static bool IsExtendedKey(KeyboardKey keyCode)
		{
			if (keyCode == KeyboardKey.Alt ||
				keyCode == KeyboardKey.LeftAlt ||
				keyCode == KeyboardKey.RightAlt ||
				keyCode == KeyboardKey.Control ||
				keyCode == KeyboardKey.RightControl ||
				keyCode == KeyboardKey.Insert ||
				keyCode == KeyboardKey.Delete ||
				keyCode == KeyboardKey.Home ||
				keyCode == KeyboardKey.End ||
				keyCode == KeyboardKey.Prior ||
				keyCode == KeyboardKey.Next ||
				keyCode == KeyboardKey.RightArrow ||
				keyCode == KeyboardKey.UpArrow ||
				keyCode == KeyboardKey.LeftArrow ||
				keyCode == KeyboardKey.DownArrow ||
				keyCode == KeyboardKey.NumLock ||
				keyCode == KeyboardKey.ControlBreak ||
				keyCode == KeyboardKey.Snapshot ||
				keyCode == KeyboardKey.Divide)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Reset the input builder with a set of new key strokes.
		/// </summary>
		/// <param name="strokes"> </param>
		public InputBuilder Reset(params KeyStroke[] strokes)
		{
			Clear();
			AddStrokes(strokes);
			return this;
		}

		/// <summary>
		/// Returns the list of <see cref="Input" /> messages as a <see cref="System.Array" /> of <see cref="Input" /> messages.
		/// </summary>
		/// <returns> The <see cref="System.Array" /> of <see cref="Input" /> messages. </returns>
		public Input[] ToArray()
		{
			return _inputList.ToArray();
		}

		internal static IEnumerable<KeyboardKey> ConvertModifierToKey(IEnumerable<KeyboardModifier> modifiers)
		{
			return ConvertModifierToKey(modifiers.ToArray());
		}

		internal static IEnumerable<KeyboardKey> ConvertModifierToKey(params KeyboardModifier[] modifiers)
		{
			if (modifiers.Any(x => x.HasFlag(KeyboardModifier.Alt)))
			{
				yield return KeyboardKey.Alt;
			}

			if (modifiers.Any(x => x.HasFlag(KeyboardModifier.LeftAlt)))
			{
				yield return KeyboardKey.LeftAlt;
			}

			if (modifiers.Any(x => x.HasFlag(KeyboardModifier.RightAlt)))
			{
				yield return KeyboardKey.RightAlt;
			}

			if (modifiers.Any(x => x.HasFlag(KeyboardModifier.Control)))
			{
				yield return KeyboardKey.Control;
			}

			if (modifiers.Any(x => x.HasFlag(KeyboardModifier.LeftControl)))
			{
				yield return KeyboardKey.LeftControl;
			}

			if (modifiers.Any(x => x.HasFlag(KeyboardModifier.RightControl)))
			{
				yield return KeyboardKey.RightControl;
			}

			if (modifiers.Any(x => x.HasFlag(KeyboardModifier.Shift)))
			{
				yield return KeyboardKey.Shift;
			}

			if (modifiers.Any(x => x.HasFlag(KeyboardModifier.LeftShift)))
			{
				yield return KeyboardKey.LeftShift;
			}

			if (modifiers.Any(x => x.HasFlag(KeyboardModifier.RightShift)))
			{
				yield return KeyboardKey.RightShift;
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the list of <see cref="Input" /> messages.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the list of <see cref="Input" /> messages.
		/// </returns>
		/// <filterpriority> 2 </filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private static MouseFlag ToMouseButtonDownFlag(MouseButton button)
		{
			switch (button)
			{
				case MouseButton.LeftButton:
					return MouseFlag.LeftDown;

				case MouseButton.MiddleButton:
					return MouseFlag.MiddleDown;

				case MouseButton.RightButton:
					return MouseFlag.RightDown;

				default:
					return MouseFlag.LeftDown;
			}
		}

		private static MouseFlag ToMouseButtonUpFlag(MouseButton button)
		{
			switch (button)
			{
				case MouseButton.LeftButton:
					return MouseFlag.LeftUp;

				case MouseButton.MiddleButton:
					return MouseFlag.MiddleUp;

				case MouseButton.RightButton:
					return MouseFlag.RightUp;

				default:
					return MouseFlag.LeftUp;
			}
		}

		#endregion
	}
}