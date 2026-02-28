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
    public class BoolToBrushConverter : IValueConverter
    {
        public BoolToBrushConverter() {
            TrueColor = new SolidColorBrush(System.Windows.Media.Colors.ForestGreen);
            FalseColor = new SolidColorBrush(System.Windows.Media.Colors.Red);
        }
        public Brush TrueColor { get; set; }

        public Brush FalseColor { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
            {
                return TrueColor;
            }
            else
            {
                return FalseColor;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
