using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.UserData;

namespace CalFinderWP7.App.Helpers
{
    public static class Extensions
    {
        public static bool Matches(this Appointment ap, string text)
        {
            return ap.Subject.Matches(text)
                || ap.Location.Matches(text)
                || ap.Details.Matches(text);
        }

        public static bool Matches(this string text, string part)
        {
            return !string.IsNullOrEmpty(text) && text.ToUpperInvariant().Contains(part);
        }
    }
}
