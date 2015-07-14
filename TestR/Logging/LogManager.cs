#region References

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TestR.Extensions;
using TestR.Web;

#endregion

namespace TestR.Logging
{
	/// <summary>
	/// The manager for logging messages to the provided loggers.
	/// </summary>
	public static class LogManager
	{
		#region Constructors

		static LogManager()
		{
			Loggers = new Collection<ILogger>();
			ReferenceId = string.Empty;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The loggers for this manager to manage write request to.
		/// </summary>
		public static ICollection<ILogger> Loggers { get; private set; }

		/// <summary>
		/// Gets or sets the reference ID that will be associated with each write.
		/// </summary>
		public static string ReferenceId { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Sets the reference ID for a specific browser and method.
		/// </summary>
		/// <param name="browser"> The browser that is about to be tested. </param>
		/// <param name="method"> The method in which the browser will be tested. </param>
		public static void UpdateReferenceId(Browser browser, string method)
		{
			ReferenceId = string.Format("{0}-{1}-{2}-{3}", DateTime.Now.ToDateId(), DateTime.Now.ToTimeId(), browser.GetType().Name, method);
		}

		/// <summary>
		/// Write a messages to the configured loggers.
		/// </summary>
		/// <param name="message"> The message to log. </param>
		/// <param name="level"> The log level for the message. </param>
		public static void Write(string message, LogLevel level)
		{
			Loggers.Where(x => x.Level <= level).ToList()
				.ForEach(x => x.Write(message, level));
		}

		#endregion
	}
}