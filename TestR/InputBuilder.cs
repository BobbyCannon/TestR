#region References

using System;
using System.Collections.Generic;
using System.Linq;
using TestR.Desktop;
using TestR.Internal;
using TestR.Internal.Inputs;
using TestR.Internal.Native;

#endregion

namespace TestR
{
	/// <summary>
	/// A helper class for building a list of <see cref="InputTypeWithData" /> messages ready to be sent to the native Windows API.
	/// </summary>
	public class InputBuilder
	{
		#region Fields

		/// <summary>
		/// The public list of <see cref="InputTypeWithData" /> messages being built by this instance.
		/// </summary>
		private readonly List<InputTypeWithData> _inputList;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="InputBuilder" /> class.
		/// </summary>
		public InputBuilder()
		{
			_inputList = new List<InputTypeWithData>();
		}

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

			var movement = new InputTypeWithData { Type = (uint) InputType.Mouse };
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

			var movement = new InputTypeWithData { Type = (uint) InputType.Mouse };
			movement.Data.Mouse.Flags = (uint) (MouseFlag.Move | MouseFlag.Absolute | MouseFlag.VirtualDesk);
			movement.Data.Mouse.X = relativeX;
			movement.Data.Mouse.Y = relativeY;

			_inputList.Add(movement);

			return this;
		}

		/// <summary>
		/// Adds the character to the list of <see cref="InputTypeWithData" /> messages.
		/// </summary>
		/// <param name="character"> The <see cref="System.Char" /> to be added to the list of <see cref="InputTypeWithData" /> messages. </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddCharacter(char character)
		{
			ushort scanCode = character;

			var down = new InputTypeWithData
			{
				Type = (uint) InputType.Keyboard,
				Data =
				{
					Keyboard =
						new KeyboardInput
						{
							KeyCode = character,
							Scan = (ushort) (NativeInput.MapVirtualKey(character, 0) & 0xFFU),
							Flags = IsExtendedKey((KeyboardKey) character) ? (uint) KeyboardFlag.ExtendedKey : 0,
							Time = 0,
							ExtraInfo = IntPtr.Zero
						}
				}
			};

			var up = new InputTypeWithData
			{
				Type = (uint) InputType.Keyboard,
				Data =
				{
					Keyboard =
						new KeyboardInput
						{
							KeyCode = character,
							Scan = (ushort) (NativeInput.MapVirtualKey(character, 0) & 0xFFU),
							Flags = (uint) ((IsExtendedKey((KeyboardKey) character) ? KeyboardFlag.ExtendedKey : 0) | KeyboardFlag.KeyUp),
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
		/// Adds a key down to the list of <see cref="InputTypeWithData" /> messages.
		/// </summary>
		/// <param name="keys"> The keys to press down. </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddKeyDown(params KeyboardKey[] keys)
		{
			keys.ForEach(key =>
			{
				var down =
					new InputTypeWithData
					{
						Type = (uint) InputType.Keyboard,
						Data =
						{
							Keyboard =
								new KeyboardInput
								{
									KeyCode = (ushort) key,
									Scan = (ushort) (NativeInput.MapVirtualKey((uint) key, 0) & 0xFFU),
									Flags = IsExtendedKey(key) ? (uint) KeyboardFlag.ExtendedKey : 0,
									Time = 0,
									ExtraInfo = IntPtr.Zero
								}
						}
					};

				_inputList.Add(down);
			});
			return this;
		}

		/// <summary>
		/// Adds a key press to the list of <see cref="InputTypeWithData" /> messages which is equivalent to a key down followed by a key up.
		/// </summary>
		/// <param name="keys"> The keys to press down. </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddKeyPress(params KeyboardKey[] keys)
		{
			keys.ForEach(x =>
			{
				AddKeyDown(x);
				AddKeyUp(x);
			});
			return this;
		}

		/// <summary>
		/// Adds modified keystroke(s) where there are multiple modifiers and multiple keys like CTRL-ALT-K-C where CTRL
		/// and ALT are the modifierKeys and K and C are the keys.
		/// The flow is Modifiers KeyDown in order, Keys Press in order, Modifiers KeyUp in reverse order.
		/// </summary>
		/// <param name="modifier"> The modifier key. </param>
		/// <param name="keys"> The list of keys to press. </param>
		public InputBuilder AddKeyPress(KeyboardModifier modifier, params KeyboardKey[] keys)
		{
			var modifierKeys = ConvertModifierToKey(modifier).ToArray();
			AddKeyDown(modifierKeys);
			AddKeyPress(keys.ToArray());
			AddKeyUp(modifierKeys);
			return this;
		}

		/// <summary>
		/// Adds modified keystroke(s) where there are multiple modifiers and multiple keys like CTRL-ALT-K-C where CTRL
		/// and ALT are the modifierKeys and K and C are the keys.
		/// The flow is Modifiers KeyDown in order, Keys Press in order, Modifiers KeyUp in reverse order.
		/// </summary>
		/// <param name="modifiers"> The list of modifier keys </param>
		/// <param name="keys"> The list of keys to simulate </param>
		public InputBuilder AddKeyPress(IEnumerable<KeyboardModifier> modifiers, params KeyboardKey[] keys)
		{
			var modifierKeys = ConvertModifierToKey(modifiers).ToArray();
			AddKeyDown(modifierKeys);
			AddKeyPress(keys.ToArray());
			AddKeyUp(modifierKeys);
			return this;
		}

		/// <summary>
		/// Adds a key up to the list of <see cref="InputTypeWithData" /> messages.
		/// </summary>
		/// <param name="keys"> The keys to release. </param>
		/// <returns> This <see cref="InputBuilder" /> instance. </returns>
		public InputBuilder AddKeyUp(params KeyboardKey[] keys)
		{
			keys.ForEach(key =>
			{
				var up =
					new InputTypeWithData
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
			});
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
			var buttonDown = new InputTypeWithData { Type = (uint) InputType.Mouse };
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
			var buttonUp = new InputTypeWithData { Type = (uint) InputType.Mouse };
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
			var scroll = new InputTypeWithData { Type = (uint) InputType.Mouse };
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
			var scroll = new InputTypeWithData { Type = (uint) InputType.Mouse };
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
			var buttonDown = new InputTypeWithData { Type = (uint) InputType.Mouse };
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
			var buttonUp = new InputTypeWithData { Type = (uint) InputType.Mouse };
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
			var movement = new InputTypeWithData { Type = (uint) InputType.Mouse };
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

		/// <summary>
		/// Add key stroke to the input builder.
		/// </summary>
		/// <param name="strokes"> The stroke(s) to add. </param>
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
		/// Returns the list of <see cref="InputTypeWithData" /> messages as a <see cref="System.Array" /> of <see cref="InputTypeWithData" /> messages.
		/// </summary>
		/// <returns> The <see cref="System.Array" /> of <see cref="InputTypeWithData" /> messages. </returns>
		internal InputTypeWithData[] ToArray()
		{
			return _inputList.ToArray();
		}

		/// <summary>
		/// Converts the modifier to the key.
		/// </summary>
		/// <param name="modifiers"> The modifiers to convert. </param>
		/// <returns> The key representation of the modifier. </returns>
		private static IEnumerable<KeyboardKey> ConvertModifierToKey(IEnumerable<KeyboardModifier> modifiers)
		{
			return ConvertModifierToKey(modifiers.ToArray());
		}

		/// <summary>
		/// Converts the modifier to the key.
		/// </summary>
		/// <param name="modifiers"> The modifiers to convert. </param>
		/// <returns> The key representation of the modifier. </returns>
		private static IEnumerable<KeyboardKey> ConvertModifierToKey(params KeyboardModifier[] modifiers)
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
		/// Determines if the <see cref="KeyboardKey" /> is an ExtendedKey
		/// </summary>
		/// <param name="keyCode"> The key code. </param>
		/// <returns> true if the key code is an extended key otherwise false. </returns>
		/// <remarks>
		/// The extended keys consist of the ALT and CTRL keys on the right-hand side of the keyboard; the INS, DEL, HOME, END, PAGE UP, PAGE DOWN, and arrow keys in the clusters to the left of the numeric keypad; the NUM LOCK key; the BREAK (CTRL+PAUSE) key; the PRINT SCRN key; and the divide (/) and ENTER keys in the numeric keypad.
		/// See http://msdn.microsoft.com/en-us/library/ms646267(v=vs.85).aspx Section "Extended-Key Flag"
		/// </remarks>
		private static bool IsExtendedKey(KeyboardKey keyCode)
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