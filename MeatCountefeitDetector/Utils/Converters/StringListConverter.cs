using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace MeatCounterfeitDetector.Utils
{
    public class StringListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<string> stringList)
            {
                return string.Join(", ", stringList);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
