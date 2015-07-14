#region References

using System;

#endregion

namespace TestR.Logging
{
	/// <summary>
	/// Represents the interface for a logger.
	/// </summary>
	public interface ILogger : IDisposable
	{
		#region Properties

		/// <summary>
		/// The max level for this logger to process.
		/// </summary>
		LogLevel Level { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Write the message to the logger.
		/// </summary>
		/// <param name="message"> The message to log. </param>
		/// <param name="level"> The log level of the message. </param>
		void Write(string message, LogLevel level);

		#endregion
	}
}