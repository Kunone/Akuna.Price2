using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using kun.PriceUI.Resource;

namespace kun.PriceUI.Converter
{
    public class StatusBarBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return ColorResource.backgroundColors[ColorResource.ColorType.Disconnected];
            if (!(value is bool))
                throw new ArgumentException("parameter must be of type boolean");

            return (bool)value ? ColorResource.backgroundColors[ColorResource.ColorType.Connected] : ColorResource.backgroundColors[ColorResource.ColorType.Disconnected];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
