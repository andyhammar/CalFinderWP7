using System;
using Microsoft.Phone.UserData;

namespace CalFinderWP7.App.Helpers
{
    public static class Extensions
    {
        private const string DateTimeFormatString = "yyyy-MM-dd";

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

        public static string ToSwedishTime(this DateTime dateTime)
        {
            return dateTime.ToString(DateTimeFormatString);
        }
    }
}
