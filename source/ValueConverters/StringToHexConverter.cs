using System;
using System.Windows.Data;

namespace iMS_Studio_first_attempt.ValueConverters
{
    class StringToHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            String str = "0x0";
            if (value is UInt32)
            {
                str = String.Format("0x{0:X4}", (uint)value);
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            String hexString = (String)value;
            UInt32 result = 0;
            if (hexString != null)
            {
                try
                {
                    // Remove any white space then convert
                    result = System.Convert.ToUInt32(hexString.Trim(), 16);
                }
                catch (FormatException)
                {
                    // Invalid format, return 0
                }
                catch (OverflowException)
                {
                    // Too big! Return 0
                }
            }
            return result;
        }
    }
}
