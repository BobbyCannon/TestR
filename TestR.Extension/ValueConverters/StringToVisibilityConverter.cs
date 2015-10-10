#region References

using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

#endregion

namespace TestR.Extension.ValueConverters
{
	public class StringToVisibilityConverter : IValueConverter
	{
		#region Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var typeValue = value.ToString();
			var parameterValues = ((string) parameter).Split(';');

			return parameterValues.Contains(typeValue) ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}