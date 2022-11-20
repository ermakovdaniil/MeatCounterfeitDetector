using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace VKR.UserInterface.Admin.Counterfeit
{
    public class SizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataAccess.Models.Counterfeit counterfeit = (DataAccess.Models.Counterfeit)value;
            return $"{counterfeit.BotLineSize}-{counterfeit.UpLineSize}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
