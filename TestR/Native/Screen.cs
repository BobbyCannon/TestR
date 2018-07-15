#region References

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

#endregion

namespace TestR.Native
{
	/// <summary>
	/// Represents the screen and allows for capturing of screen data.
	/// </summary>
	public static class Screen
	{
		#region Constants

		/// <summary>
		/// The prefix for a data url.
		/// </summary>
		public const string DataUrlPrefix = "data:image/png;base64,";

		#endregion

		#region Methods

		/// <summary>
		/// Captures a section of the screen.
		/// </summary>
		/// <param name="location"> The upper left starting location. </param>
		/// <param name="size"> The size to capture. </param>
		/// <returns> The image of the screen section. </returns>
		public static byte[] Capture(Point location, Size size)
		{
			var result = new Bitmap(size.Width, size.Height);

			using (var graphics = Graphics.FromImage(result))
			{
				graphics.CopyFromScreen(location, Point.Empty, size);
				return ConvertToByteArray(result);
			}
		}

		/// <summary>
		/// Converts a image to a data url.
		/// </summary>
		/// <returns> The data url for the image. </returns>
		public static byte[] ConvertFromDataUrl(string data)
		{
			var bytes = Convert.FromBase64String(data.Remove(0, DataUrlPrefix.Length));
			using (var stream = new MemoryStream(bytes))
			{
				return stream.ToArray();
			}
		}

		/// <summary>
		/// Converts a image to a byte array.
		/// </summary>
		/// <param name="image"> The image to convert. </param>
		/// <returns> The data for the image. </returns>
		public static byte[] ConvertToByteArray(Image image)
		{
			using (var stream = new MemoryStream())
			{
				image.Save(stream, ImageFormat.Png);
				return stream.ToArray();
			}
		}

		/// <summary>
		/// Converts a image to a data url.
		/// </summary>
		/// <param name="filePath"> The file path to the image to convert. </param>
		/// <returns> The data url for the image. </returns>
		public static string ConvertToDataUrl(string filePath)
		{
			using (var image = Image.FromFile(filePath))
			{
				return ConvertToDataUrl(image);
			}
		}

		/// <summary>
		/// Converts a image to a data url.
		/// </summary>
		/// <param name="image"> The image to convert. </param>
		/// <returns> The data url for the image. </returns>
		public static string ConvertToDataUrl(Image image)
		{
			return ConvertToDataUrl(ConvertToByteArray(image));
		}

		/// <summary>
		/// Converts a image data to a data url.
		/// </summary>
		/// <param name="data"> The image data to convert. </param>
		/// <returns> The data url for the image. </returns>
		public static string ConvertToDataUrl(byte[] data)
		{
			return $"{DataUrlPrefix}{Convert.ToBase64String(data)}";
		}

		#endregion
	}
}