using ImageWorker.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MeatCountefeitDetector.Utils
{
    public class EnumToDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Priorities priority)
            {
                switch (priority)
                {
                    case Priorities.Accuracy:
                        return "Точность";
                    case Priorities.Performance:
                        return "Производительность";
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
