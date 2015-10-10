#region References

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#endregion

namespace TestR.Extension.ValueConverters
{
	public class BoolToVisibilityConverter : IValueConverter
	{
		#region Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool) value ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}