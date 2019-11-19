using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MyLeasing.Common.Helpers;
using MyLeasing.Common.Models;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MyLeasing.Prism.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        public ICommand CalculateRouteCommand { get; set; }
        public ICommand UpdatePositionCommand { get; set; }
        public ICommand LoadRouteCommand { get; set; }
        public ICommand StopRouteCommand { get; set; }
        public bool HasRouteRunning { get; set; }

        GooglePlaceAutoCompletePrediction _placeSelected;

        public event PropertyChangedEventHandler PropertyChanged;
        public GooglePlaceAutoCompletePrediction PlaceSelected
        {
            get
            {
                return _placeSelected;
            }
            set
            {
                _placeSelected = value;
                if (_placeSelected != null)
                    GetPlaceDetailCommand.Execute(_placeSelected);
            }
        }
        public ICommand FocusOriginCommand { get; set; }
        public ICommand GetPlacesCommand { get; set; }
        public ICommand GetPlaceDetailCommand { get; set; }

        public ObservableCollection<GooglePlaceAutoCompletePrediction> Places { get; set; }
        public ObservableCollection<GooglePlaceAutoCompletePrediction> RecentPlaces { get; set; } = new ObservableCollection<GooglePlaceAutoCompletePrediction>();

        public bool ShowRecentPlaces { get; set; }

        public ICommand GetLocationNameCommand { get; set; }
        public bool IsRouteNotRunning
        {
            get
            {
                return !HasRouteRunning;
            }
        }

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GetLocationNameCommand = new Command<Position>(async (param) => await GetLocationName(param));
        }

        //Get place 
        public async Task GetLocationName(Position position)
        {
            try
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(position.Latitude, position.Longitude);
                Settings.Latitude = position.Latitude.ToString();
                Settings.Longitude = position.Longitude.ToString();
                await _navigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
