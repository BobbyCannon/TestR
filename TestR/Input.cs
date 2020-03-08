#region References

using TestR.Desktop;

#endregion

namespace TestR
{
	/// <summary>
	/// A static combination a keyboard and mouse instance.
	/// </summary>
	public static class Input
	{
		#region Constructors

		static Input()
		{
			Keyboard = new Keyboard();
			Mouse = new Mouse();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Represents the keyboard and allows for simulated input.
		/// </summary>
		public static Keyboard Keyboard { get; }

		/// <summary>
		/// Represents the mouse and allows for simulated input.
		/// </summary>
		public static Mouse Mouse { get; }

		#endregion
	}
}