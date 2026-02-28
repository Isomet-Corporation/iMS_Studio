using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace iMS_Studio.ValueConverters
{
    /// <summary>
    /// Returns false for true and vice versa
    /// </summary>
    public class BooleanInvertValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = System.Convert.ToBoolean(value);
            return !val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = System.Convert.ToBoolean(value);
            return !val;
        }
    }
}
