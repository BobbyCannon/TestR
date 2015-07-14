#region References

using System;
using System.Diagnostics;
using System.Threading;

#endregion

namespace TestR.Helpers
{
	/// <summary>
	/// Represents the Utility class.
	/// </summary>
	public static class Utility
	{
		#region Methods

		/// <summary>
		/// Runs the action until the action returns true or the timeout is reached. Will delay in between actions of the provided
		/// time.
		/// </summary>
		/// <param name="action"> The action to call. </param>
		/// <param name="timeout"> The timeout to attempt the action. This value is in milliseconds. </param>
		/// <param name="delay"> The delay in between actions. This value is in milliseconds. </param>
		/// <returns> Returns true of the call completed successfully or false if it timed out. </returns>
		public static bool Wait(Func<bool> action, double timeout = 1000, int delay = 50)
		{
			var watch = Stopwatch.StartNew();
			var watchTimeout = TimeSpan.FromMilliseconds(timeout);
			var result = false;

			while (!result)
			{
				if (watch.Elapsed > watchTimeout)
				{
					return false;
				}

				result = action();
				if (!result)
				{
					Thread.Sleep(delay);
				}
			}

			return true;
		}

		/// <summary>
		/// Runs the action until the action returns true or the timeout is reached. Will delay in between actions of the provided
		/// time.
		/// </summary>
		/// <param name="input"> The input to pass to the action. </param>
		/// <param name="action"> The action to call. </param>
		/// <param name="timeout"> The timeout to attempt the action. This value is in milliseconds. </param>
		/// <param name="delay"> The delay in between actions. This value is in milliseconds. </param>
		/// <returns> Returns true of the call completed successfully or false if it timed out. </returns>
		public static bool Wait<T>(T input, Func<T, bool> action, int timeout = 1000, int delay = 50)
		{
			var watch = Stopwatch.StartNew();
			var watchTimeout = TimeSpan.FromMilliseconds(timeout);
			var result = false;

			while (!result)
			{
				if (watch.Elapsed > watchTimeout)
				{
					return false;
				}

				result = action(input);
				if (!result)
				{
					Thread.Sleep(delay);
				}
			}

			return true;
		}

		#endregion
	}
}