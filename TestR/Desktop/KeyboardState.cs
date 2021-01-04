namespace TestR.Desktop
{
	/// <summary>
	/// Represents the state of the keyboard during Keyboard.StartMonitoring.
	/// </summary>
	public class KeyboardState
	{
		#region Properties

		/// <summary>
		/// The string interpretation of the key.
		/// </summary>
		public char Character { get; set; }

		/// <summary>
		/// Gets a value indicating if either the left or right alt key is pressed.
		/// </summary>
		public bool IsAltPressed => IsLeftAltPressed || IsRightAltPressed;

		/// <summary>
		/// Determines if the caps lock in on at the time of the key event.
		/// </summary>
		public bool IsCapsLockOn { get; set; }

		/// <summary>
		/// Gets a value indicating if either the left or right control key is pressed.
		/// </summary>
		public bool IsControlPressed => IsLeftControlPressed || IsRightControlPressed;

		/// <summary>
		/// Gets a value indicating if the left alt key is pressed.
		/// </summary>
		public bool IsLeftAltPressed { get; set; }

		/// <summary>
		/// Gets a value indicating if the left control key is pressed.
		/// </summary>
		public bool IsLeftControlPressed { get; set; }

		/// <summary>
		/// Gets a value indicating if the left shift key is pressed.
		/// </summary>
		public bool IsLeftShiftPressed { get; set; }

		/// <summary>
		/// Gets a value indicating if the keyboard input is being monitored.
		/// </summary>
		public bool IsMonitoring { get; set; }

		/// <summary>
		/// Gets a value indicating the key is being pressed (down). If false the key is being released (up).
		/// </summary>
		public bool IsPressed { get; set; }

		/// <summary>
		/// Gets a value indicating if the right alt key is pressed.
		/// </summary>
		public bool IsRightAltPressed { get; set; }

		/// <summary>
		/// Gets a value indicating if the right control key is pressed.
		/// </summary>
		public bool IsRightControlPressed { get; set; }

		/// <summary>
		/// Gets a value indicating if the right shift key is pressed.
		/// </summary>
		public bool IsRightShiftPressed { get; set; }

		/// <summary>
		/// Gets a value indicating if either the left or right shift key is pressed.
		/// </summary>
		public bool IsShiftPressed => IsLeftShiftPressed || IsRightShiftPressed;

		/// <summary>
		/// Gets a value of the key being changed (up or down).
		/// </summary>
		public KeyboardKey Key { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Clone the keyboard state.
		/// </summary>
		/// <returns> The copy of the keyboard state. </returns>
		public KeyboardState Clone()
		{
			return new KeyboardState
			{
				Character = Character,
				IsCapsLockOn = IsCapsLockOn,
				IsLeftAltPressed = IsLeftAltPressed,
				IsLeftControlPressed = IsLeftControlPressed,
				IsLeftShiftPressed = IsLeftShiftPressed,
				IsMonitoring = IsMonitoring,
				IsPressed = IsPressed,
				IsRightAltPressed = IsRightAltPressed,
				IsRightControlPressed = IsRightControlPressed,
				IsRightShiftPressed = IsRightShiftPressed,
				Key = Key
			};
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return Character == (char) 0 ? Keyboard.ToCharacter(Key, this).ToString() : Character.ToString();
		}

		#endregion
	}
}