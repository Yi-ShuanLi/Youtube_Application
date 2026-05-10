using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Youtube_Application.Converters
{
    public class ViewCountToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long count = (long)value;
            if (count < 10000)
                return $"{count.ToString("N0")}次觀看";
            double DivideTenThousand = (double)count / 10000;
            double Rounding = Math.Round(DivideTenThousand, 2);
            return $"{Rounding.ToString("#,##0.00")}萬次觀看";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
