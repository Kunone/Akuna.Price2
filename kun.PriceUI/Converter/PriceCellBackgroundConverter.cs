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
    public class PriceCellBackgroundConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return ColorResource.backgroundColors[ColorResource.ColorType.PriceEqual];

            if (!(value[0] is double) || !(value[1] is double))
                return ColorResource.backgroundColors[ColorResource.ColorType.PriceEqual];

            if ((double)value[0] < (double)value[1])
                return ColorResource.backgroundColors[ColorResource.ColorType.PriceLower];
            else if((double)value[0] > (double)value[1])
                return ColorResource.backgroundColors[ColorResource.ColorType.PriceHigher];
            else
                return ColorResource.backgroundColors[ColorResource.ColorType.PriceEqual];
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}