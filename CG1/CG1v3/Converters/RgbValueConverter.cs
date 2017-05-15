using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CG1v3.Converters
{
    class RgbValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double && targetType == typeof(string))
            {
                var d = System.Convert.ToDouble(value);

                return ((int) (d * 255)).ToString();
            }
            if (value is string && targetType == typeof(double))
            {
                var b = System.Convert.ToInt32(value);

                if (b > 255 || b < 0)
                    throw new ArgumentException("value is malformed", "value");

                return b / 255.0;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
