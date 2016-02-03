using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace kun.PriceUI.Resource
{
    public static class ColorResource
    {
        public static Dictionary<ColorType, SolidColorBrush> backgroundColors = new Dictionary<ColorType, SolidColorBrush>();

        static ColorResource()
        {
            backgroundColors.Add(ColorType.PriceHigher, new SolidColorBrush(Color.FromRgb(0, 176, 240))); //#FF00B0F0
            backgroundColors.Add(ColorType.PriceLower, new SolidColorBrush(Color.FromRgb(255, 0, 0))); //#FFFF0000
            backgroundColors.Add(ColorType.PriceEqual, new SolidColorBrush(Color.FromRgb(255, 255, 255))); //#FFFFFFFF
            backgroundColors.Add(ColorType.Connected, new SolidColorBrush(Color.FromRgb(0, 176, 80)));
            backgroundColors.Add(ColorType.Disconnected, new SolidColorBrush(Color.FromRgb(255, 255, 0)));
        }

        public enum ColorType
        {
            PriceHigher,
            PriceLower,
            PriceEqual,
            Connected,
            Disconnected
        }
    }
}
