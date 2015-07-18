#region References

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#endregion

namespace TestR.Editor.ValueConverters
{
	public class StringToVisibilityConverter : IValueConverter
	{
		#region Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var typeValue = value as string;
			return string.IsNullOrWhiteSpace(typeValue) ? Visibility.Hidden : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}