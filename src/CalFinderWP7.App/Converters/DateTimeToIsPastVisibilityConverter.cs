using System;
using System.Windows;
using System.Windows.Data;
using CalFinderWP7.App.Resources;

namespace CalFinderWP7.App.Converters
{
    public class DateTimeToIsPastVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var date = (DateTime) value;

            
            var isPast = date < DateTime.Now;

            return isPast ? Visibility.Visible : Visibility.Collapsed;
            //return isAllDay ? AppRes.IsAllDayText : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
