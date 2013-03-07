using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using Microsoft.Phone.UserData;
using CalFinderWP7.App.Helpers;
using CalFinderWP7.App.Resources;

namespace CalFinderWP7.App.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            Appointments = new ObservableCollection<Appointment>();
        }

        public string AppName { get { return AppRes.AppTitle; } }

        public bool LightThemeEnabled
        {
            get
            {
                return (Visibility)Application.Current.Resources["PhoneLightThemeVisibility"] == Visibility.Visible;
            }
        }

        public Brush BackgroundBrush
        {
            get
            {
                if (App.ViewModel.LightThemeEnabled)
                {
                    return Application.Current.Resources["AppLightGradientBackgroundBrush"] as Brush;
                }
                return Application.Current.Resources["AppDarkGradientBackgroundBrush"] as Brush;
            }
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
            _searchPeriods = new Dictionary<DateTime, DateTime>
                {
                    {_now, _now.AddMonths(1)},
                    {_now.AddMonths(1), _now.AddYears(12)},
                    {_now.AddMonths(-1), _now},
                    {_now.AddMonths(-2), _now.AddMonths(-1)},
                    {_now.AddMonths(-3), _now.AddMonths(-2)}
                };

            LaunchSearch(term);
            Navigate.ToSearchResultPage();
        }

        private void LaunchSearch(string term)
        {
            StatusText = string.Format(AppRes.BusyText_searchTerm, term);
            IsBusy = true;
            _appointments = new Appointments();
            _appointments.SearchCompleted += appointments_SearchCompleted;

            var first = Pop();

            LaunchSearchForPeriod(term, first.Key, first.Value);
        }

        private KeyValuePair<DateTime, DateTime> Pop()
        {
            var first = _searchPeriods.First();
            _searchPeriods.Remove(first.Key);
            return first;
        }

        private void LaunchSearchForPeriod(string term, DateTime from, DateTime to)
        {
            _appointments.SearchAsync(from, to, 1000, term);
        }

        void appointments_SearchCompleted(object sender, AppointmentsSearchEventArgs e)
        {
            var searchTerm = e.State as string;
            if (searchTerm == null) { return; }



            var wasLastSearch = false;
            if (WasLastSearch())
            {
                wasLastSearch = true;
            }
            else
            {
                var next = Pop();
                var from = next.Key;
                var to = next.Value;
                LaunchSearchForPeriod(searchTerm, from, to);
            }

            var searchTermUpper = searchTerm.ToUpperInvariant();
            foreach (var ap in e.Results)
            {
                if (!ap.Matches(searchTermUpper)) continue;

                var firstNewerItem = Appointments.FirstOrDefault(x => x.StartTime > ap.StartTime);
                if (firstNewerItem == null)
                {
                    Appointments.Add(ap);
                }
                else
                {
                    var index = Appointments.IndexOf(firstNewerItem);
                    Appointments.Insert(index, ap);
                }

            }

            if (wasLastSearch)
            {
                IsBusy = false;
                if (!Appointments.Any())
                {
                    StatusText = string.Format(AppRes.NothingFoundMessage_searchTerm_from_to,
                        searchTerm,
                        _now.ToSwedishTime(),
                        e.EndTimeInclusive.ToSwedishTime());
                }
                else
                {
                    StatusText = null;
                }
            }
        }

        private bool WasLastSearch()
        {
            return !_searchPeriods.Any();
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
        private Dictionary<DateTime, DateTime> _searchPeriods;
        private Appointments _appointments;
    }

}