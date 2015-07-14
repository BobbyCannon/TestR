#region References

using System;

#endregion

namespace TestR.Extensions
{
	public static partial class Helper
	{
		#region Methods

		/// <summary>
		/// Converts the date of the provided date time into an integer ID in the format of 'yyyyMMdd'.
		/// </summary>
		/// <param name="time"> The date and time input. </param>
		/// <returns> The ID of the date in an integer format. </returns>
		public static int ToDateId(this DateTime time)
		{
			return int.Parse(time.ToString("yyyyMMdd"));
		}

		/// <summary>
		/// Calculates a time that is valid for SQL. We will trim the milliseconds and convert the time to Unspecified kind.
		/// </summary>
		/// <param name="dateTime"> The date time to convert to SQL compatible version. </param>
		/// <returns> The date time that is compatible with SQL. </returns>
		public static DateTime ToSqlDateTime(this DateTime dateTime)
		{
			var response = dateTime.Truncate(TimeSpan.FromSeconds(1));
			return DateTime.SpecifyKind(response, DateTimeKind.Unspecified);
		}

		/// <summary>
		/// Converts the time of the provided date time into an integer ID in the format of 'HHmmss'.
		/// </summary>
		/// <param name="time"> The date and time input. </param>
		/// <returns> The ID of the time in an integer format. </returns>
		public static int ToTimeId(this DateTime time)
		{
			return int.Parse(time.ToString("HHmmss"));
		}

		/// <summary>
		/// Truncate the time with the time span provided.
		/// </summary>
		/// <param name="dateTime"> The date time to truncate. </param>
		/// <param name="timeSpan"> The amount of time to truncate. </param>
		/// <returns> The truncated date time. </returns>
		public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
		{
			return timeSpan == TimeSpan.Zero ? dateTime : dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
		}

		#endregion
	}
}