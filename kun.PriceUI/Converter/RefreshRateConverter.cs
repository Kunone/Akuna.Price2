using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace kun.PriceUI.Converter
{
    public class RefreshRateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;

            if (!(value is double)) return string.Empty;

            if ((double)value < 1000)
                return string.Format("Refresh Rate: {0:F} ms", value);
            else
                return string.Format("Refresh Rate: {0:F} s", (double)value / 1000);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
