using System;
using System.Collections.Generic;
using MyLeasing.Common.Helpers;
using MyLeasing.Common.Models;
using MyLeasing.Common.Services;
using MyLeasing.Prism.Helpers;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MyLeasing.Prism.Views
{
    public partial class MapPage : ContentPage
    {
        private readonly IGeolocatorService _geolocatorService;
        private readonly IApiService _apiService;
        public MapPage(IGeolocatorService geolocatorService,
            IApiService apiService)
        {
            InitializeComponent();
            mp.MapType = MapType.Street;
            ma2.Source = "satellite.png";
            _geolocatorService = geolocatorService;
            _apiService = apiService;      
            ShowPropertiesAsync(); 
            MoveMapToCurrentPositionAsync();  
        }
        private void Maptype(object sender, EventArgs e)
        {
            if (mp.MapType == MapType.Street)
            {
                mp.MapType = MapType.Satellite;
                ma2.Source = "street.png";
                return;
            }
            mp.MapType = MapType.Street;
            ma2.Source = "satellite.png";
        }
        private async void ShowPropertiesAsync()
        {

            var url = URL.UrlAPI;
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var response = await _apiService.GetListAsync<PropertyResponse>(
                url,
                "/api",
                "/Owners/GetAvailbleProperties",
                "bearer",
                token.Token);

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Ok);
                return;
            }

            var properties = (List<PropertyResponse>)response.Result;

            foreach (var property in properties)
            {
                mp.Pins.Add(new Pin
                {
                    Address = property.Address,
                    Label = property.Neighborhood,
                    Position = new Position(property.Latitude, property.Longitude),
                    Type = PinType.Place
                });
            }
        }

        private async void MoveMapToCurrentPositionAsync()
        {
            await CrossGeolocator.Current.GetLastKnownLocationAsync();
            if (!CrossGeolocator.Current.IsGeolocationEnabled)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.GPS, Languages.Ok);
            }
            await _geolocatorService.GetLocationAsync();
            if (_geolocatorService.Latitude != 0 && _geolocatorService.Longitude != 0)
            {
                var position = new Position(
                    _geolocatorService.Latitude,
                    _geolocatorService.Longitude);
                mp.MoveToRegion(MapSpan.FromCenterAndRadius(
                    position,
                    Distance.FromKilometers(1)));
            }
        }
    }
}

