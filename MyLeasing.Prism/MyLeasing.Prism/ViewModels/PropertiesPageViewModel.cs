using MyLeasing.Common.Helpers;
using MyLeasing.Common.Models;
using MyLeasing.Common.Services;
using MyLeasing.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyLeasing.Prism.ViewModels
{
    public class PropertiesPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;
        private OwnerResponse _owner;
        private ObservableCollection<PropertyItemViewModel> _properties;
        private DelegateCommand _addPropertyCommand;
        private static PropertiesPageViewModel _instance;
        private DelegateCommand _refreshPropertiesCommand;
        private bool _IsRefreshing;
        public PropertiesPageViewModel(
               INavigationService navigationService,
               IApiService apiService) : base(navigationService)
        {
            _instance = this;
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Properties;
            IsRefreshing = false;
            LoadOwner();
        }

        public DelegateCommand AddPropertyCommand => _addPropertyCommand ?? (_addPropertyCommand = new DelegateCommand(AddProperty));
        public DelegateCommand RefreshPropertiesCommand => _refreshPropertiesCommand ?? (_refreshPropertiesCommand = new DelegateCommand(RefreshProperties));

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            UpdateOwner();
        }
        private async void AddProperty()
        {
            if (_owner.RoleId != 1)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorNoOwner, Languages.Ok);
                return;
            }
           
            await _navigationService.NavigateAsync("EditProperty");
        }
        public bool IsRefreshing
        {
            get => _IsRefreshing;
            set => SetProperty(ref _IsRefreshing, value);
        }
        private async void RefreshProperties()
        {
            IsRefreshing = true;
            await UpdateOwner();
            IsRefreshing = false;
        }

        public ObservableCollection<PropertyItemViewModel> Properties
        {
            get => _properties;
            set => SetProperty(ref _properties, value);
        }

        private void LoadOwner()
        {
            _owner = JsonConvert.DeserializeObject<OwnerResponse>(Settings.Owner);
            if (_owner.RoleId == 1)
            {
                Title = Languages.Properties + " : " + _owner.FullName;
            }
            else
            {
                Title = Languages.Properties1;
            }
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
                Latitude=p.Latitude,
                Longitude=p.Longitude,
                Typeprop=p.Typeprop
            }).ToList());
        }
        public static PropertiesPageViewModel GetInstance()
        {
            return _instance;
        }

        public async Task UpdateOwner()
        {
            var url = URL.UrlAPI;
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var response = await _apiService.GetOwnerByEmailAsync(
                url,
                "/api",
                "/Owners/GetOwnerByEmail",
                "bearer",
                token.Token,
                _owner.Email);

            if (response.IsSuccess)
            {
                var owner = (OwnerResponse)response.Result;
                Settings.Owner = JsonConvert.SerializeObject(owner);
                _owner = owner;
                LoadOwner();
            }
        }
    }
}
