#region References

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace TestR.Extension.ValueConverters
{
	public class IntToBoolConverter : IValueConverter
	{
		#region Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (int) value > 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}