using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyLeasing.Prism.Views
{
    public partial class PropertiesPage : ContentPage
    {
        public PropertiesPage()
        {
            InitializeComponent();
        }
        private async void Clic(object sender, EventArgs e)
        {
            await ((Button)sender).ScaleTo(0, 50, Easing.Linear);
            await Task.Delay(100);
            await ((Button)sender).ScaleTo(1, 50, Easing.Linear);
        }
        }
    }
