using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Microsoft.Phone.UserData;
using CalFinderWP7.App.Helpers;
using CalFinderWP7.App.Resources;


namespace CalFinderWP7.App
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            Appointments = new ObservableCollection<Appointment>();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal void Search(string term)
        {
            if (string.IsNullOrEmpty(term)) return;
            LaunchSearch(term);
            Navigate.ToSearchResultPage();
        }

        private void LaunchSearch(string term)
        {
            Appointments.Clear();
            BusyText = AppRes.BusyText;
            var appointments = new Appointments();
            appointments.SearchCompleted += new EventHandler<AppointmentsSearchEventArgs>(appointments_SearchCompleted);
            appointments.SearchAsync(DateTime.Now, DateTime.Now.AddYears(1), term);
        }

        void appointments_SearchCompleted(object sender, AppointmentsSearchEventArgs e)
        {
            BusyText = null;
            var searchTerm = e.State as string;
            searchTerm = searchTerm.ToUpperInvariant();
            foreach (var ap in e.Results)
            {
                if (!ap.Matches(searchTerm)) continue;
                Appointments.Add(ap);
            }
        }

        public ObservableCollection<Appointment> Appointments { get; set; }

        public bool IsDataLoaded { get; set; }

        internal void LoadData()
        {
            //load previous searches
        }

        public string BusyText
        {
            get
            {
                return _busyText;
            }
            set
            {
                if (value != _busyText)
                {
                    _busyText = value;
                    NotifyPropertyChanged("BusyText");
                }
            }
        }
        private string _busyText;
    }

}