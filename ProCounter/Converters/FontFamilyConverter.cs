using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ProCounter.Converters
{
    class FontFamilyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? new FontFamily(value?.ToString()) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }
    }
}
