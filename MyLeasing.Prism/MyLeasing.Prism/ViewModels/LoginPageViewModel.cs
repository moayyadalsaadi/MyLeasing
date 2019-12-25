﻿using MyLeasing.Common.Helpers;
using MyLeasing.Common.Models;
using MyLeasing.Common.Services;
using MyLeasing.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;

namespace MyLeasing.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _loginCommand;
        private DelegateCommand _registerCommand;
        private DelegateCommand _forgotPasswordCommand;
        public LoginPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Login";
            IsEnabled = true;
            IsRemember = true;
        }

        [Obsolete]
        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));
        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(Register));
        public DelegateCommand ForgotPasswordCommand => _forgotPasswordCommand ?? (_forgotPasswordCommand = new DelegateCommand(ForgotPassword));
        public bool IsRemember { get; set; }

        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
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

        [Obsolete]
        private async void Login()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.EmailError, Languages.Ok);
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordError, Languages.Ok);
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var url = URL.UrlAPI;
            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsEnabled = true;
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.connection, Languages.Ok);
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
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LoginError, Languages.Ok);
                Password = string.Empty;
                return;
            }

            var token = response.Result;
            var response2 = await _apiService.GetOwnerByEmailAsync(url, "/api", "/Owners/GetOwnerByEmail", "bearer", token.Token, Email);
            if (!response2.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Support, Languages.Ok);
                return;
            }

            var owner = response2.Result;

            Settings.Owner = JsonConvert.SerializeObject(owner);
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.IsRemembered = IsRemember;

            await _navigationService.NavigateAsync("/LeasingMasterDetailPage/NavigationPage/PropertiesPage");
            IsRunning = false;
            IsEnabled = true;
        }
        private async void Register()
        {
            await _navigationService.NavigateAsync("RegisterPage");
        }
        private async void ForgotPassword()
        {
            await _navigationService.NavigateAsync("RememberPasswordPage");
        }
    }
}
