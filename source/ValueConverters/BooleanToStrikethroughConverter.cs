using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace iMS_Studio.ValueConverters
{
    /// <summary>
    /// Converts a Boolean into a Visibility.
    /// </summary>
    public class BooleanToStrikethroughConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = System.Convert.ToBoolean(value);
            if (val)
            {
                return TextDecorations.Strikethrough;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
 
