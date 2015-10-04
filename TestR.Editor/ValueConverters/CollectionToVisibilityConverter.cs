using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace TestR.Editor.ValueConverters
{
	public class CollectionToVisibilityConverter : IValueConverter
	{
		#region Methods

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var collection = value as IEnumerable<object>;
			return collection?.Any() == true ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}