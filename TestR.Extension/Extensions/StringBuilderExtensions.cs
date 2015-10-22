#region References

using System.Text;

#endregion

namespace TestR.Extension.Extensions
{
	public static class StringBuilderExtensions
	{
		#region Methods

		public static void AppendFirst(this StringBuilder builder, params string[] values)
		{
			foreach (var value in values)
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					continue;
				}

				builder.Append(value);
				return;
			}
		}

		public static void AppendIf(this StringBuilder builder, string value, bool condition)
		{
			if (condition)
			{
				builder.Append(value);
			}
		}

		public static void AppendLineIf(this StringBuilder builder, string value, bool condition)
		{
			if (condition)
			{
				builder.AppendLine(value);
			}
		}

		#endregion
	}
}