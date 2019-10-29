using MyLeasing.Common.Helpers;
using MyLeasing.Common.Models;
using MyLeasing.Prism.Helpers;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyLeasing.Prism.ViewModels
{
    public class PropertyPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private PropertyResponse _property;
        private ObservableCollection<RotatorModel> _imageCollection;
        private DelegateCommand _editPropertyCommand;
        private bool _IsVisible;
        private OwnerResponse _owner;
        public PropertyPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = Languages.Details;
            Load();
        }
        public bool IsVisible
        {
            get => _IsVisible;
            set => SetProperty(ref _IsVisible, value);
        }
        private void Load()
        {
            _owner = JsonConvert.DeserializeObject<OwnerResponse>(Settings.Owner);
            if (_owner.RoleId == 1)
            {
                IsVisible = true;
            }
            else
            {
                IsVisible = false;
            }
        }
        public DelegateCommand EditPropertyCommand => _editPropertyCommand ?? (_editPropertyCommand = new DelegateCommand(EditProperty));

        public PropertyResponse Property
        {
            get => _property;
            set => SetProperty(ref _property, value);
        }
        
        public ObservableCollection<RotatorModel> ImageCollection
        {
            get => _imageCollection;
            set => SetProperty(ref _imageCollection, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            Property = JsonConvert.DeserializeObject<PropertyResponse>(Settings.Property);
            LoadImages();
        }
        private async void EditProperty()
        {
            var parameters = new NavigationParameters
            {
                { "property", _property }
            };

            await _navigationService.NavigateAsync("EditProperty", parameters);
        }
        private void LoadImages()
        {
            ImageCollection = new ObservableCollection<RotatorModel>(Property.PropertyImages.Select(pi => new RotatorModel
            {
                Image = pi.ImageUrl
            }).ToList());
        }
    }
}
