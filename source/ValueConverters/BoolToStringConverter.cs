using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace iMS_Studio.ValueConverters
{
    public class BoolToStringConverter : IValueConverter
    {
        public BoolToStringConverter()
        {
            TrueString = "True";
            FalseString = "False";
        }
        public String TrueString { get; set; }

        public String FalseString { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
            {
                return TrueString;
            }
            else
            {
                return FalseString;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
