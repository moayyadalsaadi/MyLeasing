using MyLeasing.Common.Helpers;
using MyLeasing.Common.Models;
using MyLeasing.Common.Services;
using MyLeasing.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;

namespace MyLeasing.Prism.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private OwnerResponse _owner;
        private DelegateCommand _saveCommand;
        private DelegateCommand _changePasswordCommand;

        public ProfileViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.MyProfile;
            IsEnabled = true;
            Owner = JsonConvert.DeserializeObject<OwnerResponse>(Settings.Owner);
        }

        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(Save));
        public DelegateCommand ChangePasswordCommand => _changePasswordCommand ?? (_changePasswordCommand = new DelegateCommand(ChangePassword));
        public OwnerResponse Owner
        {
            get => _owner;
            set => SetProperty(ref _owner, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void Save()
        {
            var isValid = await ValidateData();
            if (!isValid)
            {
                return;
            }
            IsRunning = true;
            IsEnabled = false;

            var userRequest = new UserRequest
            {
                Address = Owner.Address,
                Document = Owner.Document,
                Email = Owner.Email,
                FirstName = Owner.FirstName,
                LastName = Owner.LastName,
                Password = "123456", // It doesn't matter what is sent here. It is only for the model to be valid
                Phone = Owner.PhoneNumber
            };

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var url = App.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.PutAsync(
                url,
                "/api",
                "/Account",
                userRequest,
                "bearer",
                token.Token);

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


            Settings.Owner = JsonConvert.SerializeObject(Owner);

            await App.Current.MainPage.DisplayAlert(
                 Languages.Accept,
                 Languages.UserUpdated,
                 Languages.Ok);
            await _navigationService.GoBackAsync();

        }

        private async Task<bool> ValidateData()
        {
            if (Owner.Document.ToString().Length != 9)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.DocumentError, Languages.Ok);
                return false;
            }

            if (string.IsNullOrEmpty(Owner.FirstName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.FirstNameError, Languages.Ok);
                return false;
            }

            if (string.IsNullOrEmpty(Owner.LastName))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LastNameError, Languages.Ok);
                return false;
            }

            if (string.IsNullOrEmpty(Owner.Address))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.AddressError, Languages.Ok);
                return false;
            }
            if (string.IsNullOrEmpty(Owner.PhoneNumber)|| Owner.PhoneNumber.Length != 10)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PhoneError, Languages.Ok);
                return false;
            }
            return true;
        }

        private async void ChangePassword()
        {
            await _navigationService.NavigateAsync("ChangePassword");
        }
    }
}

