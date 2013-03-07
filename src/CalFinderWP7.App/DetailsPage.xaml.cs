using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace CalFinderWP7.App
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        // Constructor
        public DetailsPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null) return;

            if (listBox.SelectedIndex == -1) return;

            listBox.SelectedIndex = -1;
        }
    }
}