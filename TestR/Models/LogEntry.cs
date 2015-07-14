#region References

using TestR.Logging;

#endregion

namespace TestR.Models
{
	/// <summary>
	/// Represents a log entry.
	/// </summary>
	public class LogEntry : Entity
	{
		#region Properties

		/// <summary>
		/// The level of the log entry.
		/// </summary>
		public LogLevel Level { get; set; }

		/// <summary>
		/// The message of the log entry.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets a reference ID for the test run for this log entry.
		/// </summary>
		public string ReferenceId { get; set; }

		#endregion
	}
}