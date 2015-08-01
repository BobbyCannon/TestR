#region References

using System.Windows;

#endregion

namespace TestR.Extensions
{
	public static partial class Helper
	{
		#region Methods

		private static Point ToPoint(this System.Drawing.Point point)
		{
			return new Point(point.X, point.Y);
		}

		private static System.Drawing.Point ToPoint(this Point point)
		{
			return new System.Drawing.Point((int) point.X, (int) point.Y);
		}

		#endregion
	}
}