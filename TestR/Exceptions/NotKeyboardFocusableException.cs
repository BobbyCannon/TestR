#region References

using System;

#endregion

namespace TestR.Exceptions
{
	/// <summary>
	/// Represents an exception that occurs when a control is not keyboard focusable.
	/// </summary>
	public class NotKeyboardFocusableException : Exception
	{
		#region Constructors

		/// <summary>
		/// Instantiates an exception when an element is not keyboard focusable.
		/// </summary>
		/// <param name="message"> </param>
		public NotKeyboardFocusableException(string message)
			: base(message)
		{
		}

		#endregion
	}
}