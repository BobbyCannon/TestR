namespace TestR.Logging
{
	/// <summary>
	/// Represents the different level of logging.
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		/// Log level for very detailed messages.
		/// </summary>
		Verbose = 0,

		/// <summary>
		/// Log level for debug messages.
		/// </summary>
		Debug = 1,

		/// <summary>
		/// Log level for information messages.
		/// </summary>
		Information = 2,

		/// <summary>
		/// Log level for warning messages.
		/// </summary>
		Warning = 3,

		/// <summary>
		/// Log level for error messages.
		/// </summary>
		Error = 4,

		/// <summary>
		/// Log level for fatal error messages.
		/// </summary>
		Fatal = 5
	}
}