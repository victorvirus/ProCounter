using System;
using System.Globalization;
using System.Windows.Data;

namespace ProCounter.Converters
{
    class DropShadowConverter : IValueConverter
    {
        private const int MaxOpacity = 100;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return (bool)value ? MaxOpacity : 0;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return (int)value == MaxOpacity;
            }

            return null;
        }
    }
}
