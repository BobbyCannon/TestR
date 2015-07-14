#region References

using System.Windows;
using Point = System.Drawing.Point;

#endregion

namespace TestR.Extensions
{
	/// <summary>
	/// Represents a static helper class.
	/// </summary>
	public static partial class Helper
	{
		#region Methods

		/// <summary>
		/// Calculates the center of the rectangle.
		/// </summary>
		/// <param name="rect"> The rectangle to get the center of. </param>
		/// <returns> The center point of the rectangle. </returns>
		public static Point Center(this Rect rect)
		{
			var topLeftX = rect.Left;
			var topRightX = rect.Right;
			return new Point((int) (topLeftX + (topRightX - topLeftX) / 2), (int) (rect.Top + (rect.Bottom - rect.Top) / 2));
		}

		#endregion
	}
}