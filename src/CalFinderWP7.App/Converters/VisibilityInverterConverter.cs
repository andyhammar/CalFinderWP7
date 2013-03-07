﻿using System;
using System.Windows;
using System.Windows.Data;
using CalFinderWP7.App.Resources;

namespace CalFinderWP7.App.Converters
{
    public class VisibilityInverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var visibility = (Visibility) value;

            return visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
