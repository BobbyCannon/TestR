﻿#region References

using System;
using System.Diagnostics;
using System.Threading;

#endregion

namespace TestR
{
	/// <summary>
	/// Represents the Utility class.
	/// </summary>
	public static class Utility
	{
		#region Constants

		/// <summary>
		/// The detail wait delay.
		/// </summary>
		public const int DefaultWaitDelay = 50;

		/// <summary>
		/// The default wait timeout.
		/// </summary>
		public const double DefaultWaitTimeout = 1000;

		#endregion

		#region Methods

		/// <summary>
		/// Runs the action until the action returns true or the timeout is reached. Will delay in between actions of the
		/// provided
		/// time.
		/// </summary>
		/// <param name="action"> The action to call. </param>
		/// <param name="timeout"> The timeout to attempt the action. This value is in milliseconds. </param>
		/// <param name="delay"> The delay in between actions. This value is in milliseconds. </param>
		/// <returns> Returns true of the call completed successfully or false if it timed out. </returns>
		public static bool Wait(Func<bool> action, double timeout = DefaultWaitTimeout, int delay = DefaultWaitDelay)
		{
			return Wait<object>(null, x => action(), timeout, delay);
		}

		/// <summary>
		/// Runs the action until the action returns true or the timeout is reached. Will delay in between actions of the
		/// provided
		/// time.
		/// </summary>
		/// <param name="input"> The input to pass to the action. </param>
		/// <param name="action"> The action to call. </param>
		/// <param name="timeout"> The timeout to attempt the action. This value is in milliseconds. </param>
		/// <param name="delay"> The delay in between actions. This value is in milliseconds. </param>
		/// <returns> Returns true of the call completed successfully or false if it timed out. </returns>
		public static bool Wait<T>(T input, Func<T, bool> action, double timeout = DefaultWaitTimeout, int delay = DefaultWaitDelay)
		{
			var watch = Stopwatch.StartNew();
			var watchTimeout = TimeSpan.FromMilliseconds(timeout);

			while (true)
			{
				if (action(input))
				{
					return true;
				}

				if (watch.Elapsed > watchTimeout)
				{
					return false;
				}

				Thread.Sleep(delay);
			}
		}

		#endregion
	}
}