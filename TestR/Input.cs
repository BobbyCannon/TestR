#region References

using System;
using TestR.Desktop;
using TestR.Internal.Inputs;

#endregion

namespace TestR
{
	/// <summary>
	/// A static combination a keyboard and mouse instance.
	/// </summary>
	public static class Input
	{
		#region Fields

		/// <summary>
		/// The instance of the <see cref="InputMessageDispatcher" /> to use for dispatching <see cref="InputTypeWithData" /> messages.
		/// </summary>
		private static readonly InputMessageDispatcher _messageDispatcher;

		#endregion

		#region Constructors

		static Input()
		{
			_messageDispatcher = new InputMessageDispatcher();

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

		#region Methods

		/// <summary>
		/// Dispatches the specified inputs from the provided InputBuilder in their specified order by issuing a single call.
		/// </summary>
		/// <param name="builder"> The builder containing the input. </param>
		public static InputBuilder SendInput(InputBuilder builder)
		{
			_messageDispatcher.DispatchInput(builder.ToArray());
			return builder;
		}
		
		#endregion
	}
}