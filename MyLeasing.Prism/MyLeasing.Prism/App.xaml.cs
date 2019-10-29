using Prism;
using Prism.Ioc;
using MyLeasing.Prism.ViewModels;
using MyLeasing.Prism.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyLeasing.Common.Services;
using MyLeasing.Common.Helpers;
using Newtonsoft.Json;
using MyLeasing.Common.Models;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MyLeasing.Prism
{
    public partial class App
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            GoogleMapsApiService.Initialize(Constants.GoogleMapsApiKey);
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            if (Settings.IsRemembered && token?.Expiration > DateTime.Now)
            {
                await NavigationService.NavigateAsync("/LeasingMasterDetailPage/NavigationPage/PropertiesPage");
            }
            else
            {
                await NavigationService.NavigateAsync("/NavigationPage/HomePage");
            }
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<IGeolocatorService, GeolocatorService>();
            containerRegistry.Register<IGoogleMapsApiService, GoogleMapsApiService>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<PropertiesPage, PropertiesPageViewModel>();
            containerRegistry.RegisterForNavigation<PropertyPage, PropertyPageViewModel>();
            containerRegistry.RegisterForNavigation<ContractsPage, ContractsPageViewModel>();
            containerRegistry.RegisterForNavigation<ContractPage, ContractPageViewModel>();
            containerRegistry.RegisterForNavigation<PropertyTabbedPage, PropertyTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<LeasingMasterDetailPage, LeasingMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterViewModel>();
            containerRegistry.RegisterForNavigation<RememberPasswordPage, RememberPasswordViewModel>();
            containerRegistry.RegisterForNavigation<Profile, ProfileViewModel>();
            containerRegistry.RegisterForNavigation<ChangePassword, ChangePasswordViewModel>();
            containerRegistry.RegisterForNavigation<EditProperty, EditPropertyViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>();
        }
    }
}
