using System;
using System.Globalization;
using System.Windows.Data;

namespace ProCounter.Converters
{
    class RowColumnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                bool inverse = bool.Parse(parameter.ToString());

                if (inverse)
                {
                    return value.ToString() != "\n";
                }

                return value.ToString() == "\n";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                bool inverse = bool.Parse(parameter.ToString());

                if(inverse)
                {
                    return (bool)value ? " " : "\n";
                }

                return (bool)value ? "\n" : " ";
            }

            return null;
        }
    }
}
