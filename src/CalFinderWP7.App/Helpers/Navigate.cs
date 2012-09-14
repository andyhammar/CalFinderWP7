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

namespace CalFinderWP7.App.Helpers
{
    public class Navigate
    {
        internal static void ToSearchResultPage()
        {
            NavigateToPage("DetailsPage");
        }

        private static void NavigateToPage(string pageName)
        {
            (Application.Current.RootVisual as Frame).Navigate(new Uri(string.Format("/{0}.xaml", pageName), UriKind.Relative));
        }
    }
}
