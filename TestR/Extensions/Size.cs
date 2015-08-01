#region References

using System.Windows;

#endregion

namespace TestR.Extensions
{
	public static partial class Helper
	{
		#region Methods

		private static Size ToSize(this System.Drawing.Size size)
		{
			return new Size(size.Width, size.Height);
		}

		private static System.Drawing.Size ToSize(this Size size)
		{
			return new System.Drawing.Size((int) size.Width, (int) size.Height);
		}

		#endregion
	}
}