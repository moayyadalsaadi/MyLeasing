using Xamarin.Forms;

namespace MyLeasing.Prism.Views
{
    public partial class EditProperty : ContentPage
    {
        public EditProperty()
        {
            InitializeComponent();
            if (Device.FlowDirection == FlowDirection.LeftToRight)
            {
                img2.Source = "ic_chevron_right.png";
                img.Source = "ic_chevron_left.png";
                return;
            } 
            if(Device.FlowDirection == FlowDirection.RightToLeft)
            {
                img.Source = "ic_chevron_right.png";
                img2.Source = "ic_chevron_left.png";
                return;
            }
        }
    }
}
