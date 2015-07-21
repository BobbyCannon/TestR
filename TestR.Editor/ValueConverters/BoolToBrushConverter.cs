#region References

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TestR.Extensions;

#endregion

namespace TestR.Editor.ValueConverters
{
	public class BoolToBrushConverter : IValueConverter
	{
		#region Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var colors = parameter.ToString().Split(",");
			if (colors.Length != 2)
			{
				throw new ArgumentException("Must provide two colors to convert.");
			}

			return new BrushConverter().ConvertFromString(colors[(bool) value ? 0 : 1]) as SolidColorBrush;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}

	public class StringToBrushConverter : IValueConverter
	{
		#region Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var colors = parameter.ToString().Split(",");
			if (colors.Length != 2)
			{
				throw new ArgumentException("Must provide two colors to convert.");
			}

			return new BrushConverter().ConvertFromString(colors[value == null || value.ToString().Length <= 0 ? 0 : 1]) as SolidColorBrush;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}