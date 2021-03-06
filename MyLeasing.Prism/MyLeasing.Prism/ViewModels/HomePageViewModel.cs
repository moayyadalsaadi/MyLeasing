﻿using MyLeasing.Common.Helpers;
using MyLeasing.Common.Models;
using MyLeasing.Common.Services;
using MyLeasing.Prism.Helpers;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyLeasing.Prism.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;
        private OwnerResponse _owner;
        private string _password;
        private bool _home;
        private bool _check;
        private bool _isRunning;
        private bool _IsRefreshing;
        private ObservableCollection<PropertyItemViewModel> _properties;
        private DelegateCommand _LoginCommand;
        private DelegateCommand _RefreshCommand;
        private static HomePageViewModel _instance;
        private DelegateCommand _refreshPropertiesCommand;


        [Obsolete]
        public HomePageViewModel(
               INavigationService navigationService,
               IApiService apiService) : base(navigationService)
        {
            _instance = this;
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Properties1;
            Check = false;
            Home = true;
            Email = "ameer@gmail.com";
            Password = "123456";
            _ = LoadOwner();
            _ = CrossGeolocator.Current.GetLastKnownLocationAsync();
        }

        public DelegateCommand LoginCommand => _LoginCommand ?? (_LoginCommand = new DelegateCommand(Login));
        public DelegateCommand RefreshPropertiesCommand => _refreshPropertiesCommand ?? (_refreshPropertiesCommand = new DelegateCommand(RefreshProperties));

        [Obsolete]
        public DelegateCommand RefreshCommand => _RefreshCommand ?? (_RefreshCommand = new DelegateCommand(Refresh));
        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public bool Home
        {
            get => _home;
            set => SetProperty(ref _home, value);
        }
        public bool Check
        {
            get => _check;
            set => SetProperty(ref _check, value);
        }
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        public bool IsRefreshing
        {
            get => _IsRefreshing;
            set => SetProperty(ref _IsRefreshing, value);
        }
        private async void Login()
        {
            await _navigationService.NavigateAsync("LoginPage");
        }

        [Obsolete]
        private async void Refresh()
        {
            Check = false;
            Home = true;
            await LoadOwner();
        }

        [Obsolete]
        private async void RefreshProperties()
        {
            IsRefreshing = true;
            await LoadOwner();
            IsRefreshing = false;
        }
        public ObservableCollection<PropertyItemViewModel> Properties
        {
            get => _properties;
            set => SetProperty(ref _properties, value);
        }

        [Obsolete]
        private async Task LoadOwner()
        {
            IsRunning = true;

            var url = URL.UrlAPI;
            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsRunning = false;
                Check = true;
                Home = false;
                return;
            }

            var request = new TokenRequest
            {
                Password = Password,
                Username = Email
            };

            var response = await _apiService.GetTokenAsync(url, "/Account", "/CreateToken", request);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                Check = true;
                Home = false;
                return;
            }

            var token = response.Result;
            var response2 = await _apiService.GetOwnerByEmailAsync(url, "/api", "/Owners/GetOwnerByEmail", "bearer", token.Token, Email);
            if (!response2.IsSuccess)
            {
                IsRunning = false;
                Check = true;
                Home = false;
                return;
            }

            var owner = response2.Result;

            Settings.Owner = JsonConvert.SerializeObject(owner);
            Settings.Token = JsonConvert.SerializeObject(token);
          
            IsRunning = false;
            
            _owner = JsonConvert.DeserializeObject<OwnerResponse>(Settings.Owner);
         
            Properties = new ObservableCollection<PropertyItemViewModel>(_owner.Properties.Select(p => new PropertyItemViewModel(_navigationService)
            {
                Address = p.Address,
                Contracts = p.Contracts,
                HasParkingLot = p.HasParkingLot,
                IsAvailable = p.IsAvailable,
                Id = p.Id,
                Neighborhood = p.Neighborhood,
                Price = p.Price,
                PropertyImages = p.PropertyImages,
                PropertyType = p.PropertyType,
                Remarks = p.Remarks,
                Rooms = p.Rooms,
                Bathrooms = p.Bathrooms,
                Balconies = p.Balconies,
                SquareMeters = p.SquareMeters,
                Stratum = p.Stratum,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                Typeprop = p.Typeprop
            }).ToList());
        }
        public static HomePageViewModel GetInstance()
        {
            return _instance;
        }
    }
}
