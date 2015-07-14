#region References

using System;
using System.IO;

#endregion

namespace TestR.Logging
{
	/// <summary>
	/// Represents a logger for a file.
	/// </summary>
	public class FileLogger : ILogger
	{
		#region Fields

		private readonly string _filePath;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of the console logger.
		/// </summary>
		public FileLogger(string filePath)
		{
			_filePath = filePath;
			Level = LogLevel.Information;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The max level for this logger to process.
		/// </summary>
		public LogLevel Level { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Clears the log file.
		/// </summary>
		public void Clear()
		{
			lock (_filePath)
			{
				File.WriteAllText(_filePath, string.Empty);
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
		}

		/// <summary>
		/// Write the message to the logger.
		/// </summary>
		/// <param name="message"> The message to log. </param>
		/// <param name="level"> The log level of the message. </param>
		public void Write(string message, LogLevel level)
		{
			lock (_filePath)
			{
				File.AppendAllText(_filePath, message + Environment.NewLine);
			}
		}

		#endregion
	}
}