using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MyLeasing.Common.Helpers;
using MyLeasing.Common.Models;
using MyLeasing.Common.Services;
using MyLeasing.Prism.Helpers;
using Plugin.Geolocator;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms.GoogleMaps;

namespace MyLeasing.Prism.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IGeolocatorService _geolocatorService; 
        private bool _isRunning;
        private bool _isEnabled;
        private Position _position;
        private string _address;

        private DelegateCommand _registerCommand;

        public RegisterViewModel(INavigationService navigationService,
            IApiService apiService,
            IGeolocatorService geolocatorService) 
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _geolocatorService = geolocatorService;
            Title = Languages.Register1;
            IsEnabled = true;
            LoadRoles();            
        }

        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(Register));

        public int Document { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }     

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }

        public Role Role { get; set; }

        public ObservableCollection<Role> Roles { get; set; }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }


        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void Register()
        {
            var isValid = await ValidateData();
            if (!isValid)
            {
                return;
            }
            IsRunning = true;
            IsEnabled = false;
            
            var request = new UserRequest
            {
                Address = Address,
                Document = Document,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Password = Password,
                Phone = Phone,
                RoleId = Role.Id,
                Latitude = _position.Latitude,
                Longitude = _position.Longitude
            };

            var url = URL.UrlAPI;
            var response = await _apiService.RegisterUserAsync(
                url,
                "/api",
                "/Account",
                request);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Ok);
                return;
            }

            await App.Current.MainPage.DisplayAlert(
                Languages.Accept,
                Languages.Message,
                Languages.Ok);
            await _navigationService.GoBackAsync();
        }

        private async Task<bool> ValidateData()
        {
            if (Document.ToString().Length != 9)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.DocumentError, Languages.Ok);
                return false;
            }

            if (string.IsNullOrEmpty(FirstName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.FirstNameError, Languages.Ok);
                return false;
            }

            if (string.IsNullOrEmpty(LastName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LastNameError, Languages.Ok);
                return false;
            }

            if (string.IsNullOrEmpty(Address))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.AddressError, Languages.Ok);
                return false;
            }
           
            await ValidateAddressAsync();
  
            if (string.IsNullOrEmpty(Email) || !RegexHelper.IsValidEmail(Email))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.EmailError, Languages.Ok);
                return false;
            }

            if (string.IsNullOrEmpty(Phone)||Phone.Length!=10)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PhoneError, Languages.Ok);
                return false;
            }

            if (Role == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.RegisterAsError, Languages.Ok);
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordError, Languages.Ok);
                return false;
            }

            if (Password.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordError2, Languages.Ok);
                return false;
            }

            if (string.IsNullOrEmpty(PasswordConfirm))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordConfirmError, Languages.Ok);
                return false;
            }

            if (!Password.Equals(PasswordConfirm))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordError3, Languages.Ok);
                return false;
            }

            return true;
        }
        private async Task<bool> ValidateAddressAsync()
        {
            _ = CrossGeolocator.Current.GetLastKnownLocationAsync();
            var geoCoder = new Geocoder();
            var locations = await geoCoder.GetPositionsForAddressAsync(Address);
            var locationList = locations.ToList();
            if (locationList.Count == 0)
            {
                var response = await App.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NotAddressFound,
                    Languages.Yes,
                    Languages.No);
                if (response)
                {                  
                    if (!CrossGeolocator.Current.IsGeolocationEnabled)
                    {
                        await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.GPS, Languages.Ok);
                        return false;
                    }
                    await _geolocatorService.GetLocationAsync();
                    if (_geolocatorService.Latitude != 0 && _geolocatorService.Longitude != 0)
                    {
                        _position = new Position(
                            _geolocatorService.Latitude,
                            _geolocatorService.Longitude);

                        var list = await geoCoder.GetAddressesForPositionAsync(_position);
                        Address = list.FirstOrDefault();
                        return true;
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(
                            Languages.Error,
                            Languages.NotLocationAvailable,
                            Languages.Ok);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            if (locationList.Count == 1)
            {
                _position = locationList.FirstOrDefault();
                return true;
            }

            if (locationList.Count > 1)
            {
                var addresses = new List<Plugin.Geolocator.Abstractions.Address>();
                var names = new List<string>();
                foreach (var location in locationList)
                {
                    var list = await geoCoder.GetAddressesForPositionAsync(location);
                    names.AddRange(list);
                    foreach (var item in list)
                    {
                        addresses.Add(new Plugin.Geolocator.Abstractions.Address
                        {
                            CountryName = item,
                            Latitude = location.Latitude,
                            Longitude = location.Longitude
                        });
                    }
                }

                var source = await App.Current.MainPage.DisplayActionSheet(
                    Languages.SelectAnAddress,
                    Languages.Cancel,
                    null,
                    names.ToArray());
                if (source == Languages.Cancel)
                {
                    return false;
                }

                Address = source;
                var address = addresses.FirstOrDefault(a => a.FeatureName == source);
                _position = new Position(address.Latitude, address.Longitude);
            }

            return true;
        }
        private void LoadRoles()
        {
            Roles = new ObservableCollection<Role>
            {
                new Role { Id = 2, Name = Languages.Lessee },
                new Role { Id = 1, Name = Languages.Owner }
            };
        }
    }
}