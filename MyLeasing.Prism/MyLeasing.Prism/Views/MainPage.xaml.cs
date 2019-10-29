using MyLeasing.Prism.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MyLeasing.Prism.Views
{
    public partial class MainPage : ContentPage
    {
        
        public MainPage()
        {
            InitializeComponent();
            GetActualLocationCommand = new Command(async () => await GetActualLocation());
            map.MapType = MapType.Street;
            Map2.Source = "satellite.png";
            but.TextColor = Color.Black;
        }

        //Center map in actual location 
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetActualLocationCommand.Execute(null);
        }

        public static readonly BindableProperty GetActualLocationCommandProperty =
            BindableProperty.Create(nameof(GetActualLocationCommand), typeof(ICommand), typeof(MainPage), null, BindingMode.TwoWay);

        public ICommand GetActualLocationCommand
        {
            get { return (ICommand)GetValue(GetActualLocationCommandProperty); }
            set { SetValue(GetActualLocationCommandProperty, value); }
        }

       
        async Task GetActualLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(
                        new Position(location.Latitude, location.Longitude), Distance.FromMiles(0.3)));

                }
            }
            catch (Exception)
            {
                await DisplayAlert(Languages.Error, Languages.GPS, Languages.Ok);
            }
        }

        private void Maptype(object sender, EventArgs e)
        {
            if (map.MapType == MapType.Street)
            {
                map.MapType = MapType.Satellite;
                Map2.Source = "street.png";
                but.TextColor = Color.White;
                return;
            }
            map.MapType = MapType.Street;
            Map2.Source = "satellite.png";
            but.TextColor = Color.Black;
        }

        private async void Locat(object sender, EventArgs e)
        {
            await GetActualLocation();
        }
    }
}
