using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Youtube_Application.Converters
{
    public class DateToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime publishDate = DateTime.Parse(value.ToString());
            DateTime today = DateTime.UtcNow;
            TimeSpan dif = today - publishDate;
            if (dif.TotalSeconds >= 1 && dif.TotalMinutes < 1)
            {
                return $"{(int)dif.TotalSeconds}秒鐘前發布";
            }
            else if (dif.TotalMinutes >= 1 && dif.TotalMinutes < 60)
            {
                return $"{(int)dif.TotalMinutes}分鐘前發布";
            }
            else if (dif.TotalMinutes >= 60 && dif.TotalDays < 1)
            {
                return $"{(int)dif.TotalMinutes / 60}小時前發布";
            }
            else if (dif.TotalDays >= 1 && dif.TotalDays < 7)
            {
                int day = (int)Math.Ceiling(dif.TotalDays);
                return $"{day}天前發布";
            }
            else if (dif.TotalDays >= 7 && dif.TotalDays < 30)
            {
                int weekCount = (int)dif.TotalDays / 7;
                return $"{weekCount}周前發布";
            }
            else if (dif.TotalDays >= 30 && dif.TotalDays < 180)
            {
                int monthCount = (int)dif.TotalDays / 30;
                return $"{monthCount}月前發布";
            }
            else if (dif.TotalDays >= 180 && dif.TotalDays < 360)
            {
                return $"半年前發布";
            }
            else
            {
                int yearCount = (int)dif.TotalDays / 360;
                return $"{yearCount}年前發布";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
