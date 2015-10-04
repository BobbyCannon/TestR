#region References

using System;
using System.Text;

#endregion

namespace TestR.Extensions
{
	public static partial class Helper
	{
		#region Methods

		public static string ToDetailedString(this Exception ex)
		{
			var builder = new StringBuilder();
			AddExceptionToBuilder(builder, ex);
			return builder.ToString();
		}

		private static void AddExceptionToBuilder(StringBuilder builder, Exception ex)
		{
			builder.Append(builder.Length > 0 ? "\r\n" + ex.Message : ex.Message);

			if (ex.InnerException != null)
			{
				AddExceptionToBuilder(builder, ex.InnerException);
			}
		}

		#endregion
	}
}