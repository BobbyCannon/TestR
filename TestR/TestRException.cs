#region References

using System;

#endregion

namespace TestR
{
	/// <summary>
	/// Custom TestR Exception instead of the generic Exception
	/// </summary>
	public class TestRException : Exception
	{
		#region Constructors

		/// <summary>
		/// Custom TestR Exception instead of the generic Exception
		/// </summary>
		/// <param name="message"> </param>
		public TestRException(string message) : base(message)
		{
		}

		#endregion
	}
}