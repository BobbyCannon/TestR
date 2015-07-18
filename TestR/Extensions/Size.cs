#region References

using System.Windows;

#endregion

namespace TestR.Extensions
{
	public static class SizeExtensions
	{
		#region Methods

		public static Size ToSize(this System.Drawing.Size size)
		{
			return new Size(size.Width, size.Height);
		}

		public static System.Drawing.Size ToSize(this Size size)
		{
			return new System.Drawing.Size((int) size.Width, (int) size.Height);
		}

		#endregion
	}
}