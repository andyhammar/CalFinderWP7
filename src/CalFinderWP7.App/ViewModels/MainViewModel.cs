using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            Appointments.Clear();
            _now = DateTime.Now;
            LaunchSearch(term, _now, _now.AddMonths(1));
            Navigate.ToSearchResultPage();
        }

        private void LaunchSearch(string term, DateTime startTimeInclusive, DateTime endTimeInclusive)
        {
            StatusText = AppRes.BusyText;
            IsBusy = true;
            var appointments = new Appointments();
            appointments.SearchCompleted += appointments_SearchCompleted;
            appointments.SearchAsync(startTimeInclusive, endTimeInclusive, 1000, term);
        }

        void appointments_SearchCompleted(object sender, AppointmentsSearchEventArgs e)
        {
            StatusText = null;
            var searchTerm = e.State as string;
            if (searchTerm == null) { return; }

            var wasLastSearch = false;
            if (e.StartTimeInclusive == _now)
            {
                LaunchSearch(searchTerm, e.EndTimeInclusive, e.EndTimeInclusive.AddYears(1));
            }
            else
            {
                wasLastSearch = true;
            }

            searchTerm = searchTerm.ToUpperInvariant();
            foreach (var ap in e.Results)
            {
                if (!ap.Matches(searchTerm)) continue;
                Appointments.Add(ap);
            }
            if (wasLastSearch)
            {
                IsBusy = false;
                if (!Appointments.Any())
                {
                    StatusText = string.Format(AppRes.NothingFoundMessage_from_to,
                                               Environment.NewLine + _now.ToSwedishTime(), 
                                               e.EndTimeInclusive.ToSwedishTime());
                }
                else
                {
                    StatusText = null;
                }
            }
        }

        public ObservableCollection<Appointment> Appointments { get; set; }

        public bool IsDataLoaded { get; set; }

        internal void LoadData()
        {
            //load previous searches
        }

        public string StatusText
        {
            get
            {
                return _statusText;
            }
            set
            {
                if (value != _statusText)
                {
                    _statusText = value;
                    NotifyPropertyChanged("StatusText");
                }
            }
        }
        private string _statusText;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                if (value != _isBusy)
                {
                    _isBusy = value;
                    NotifyPropertyChanged("IsBusy");
                }
            }
        }
        private bool _isBusy;

        public string SearchInstruction { get { return AppRes.SearchInstruction; } }

        private DateTime _now;
    }

}