using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ProCounter.Converters
{
    class FontColorConverterToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? (Brush)new BrushConverter().ConvertFromString(value.ToString()) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }
    }
}

