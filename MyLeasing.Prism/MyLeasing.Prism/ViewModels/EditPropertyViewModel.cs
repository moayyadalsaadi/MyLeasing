using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MyLeasing.Common.Helpers;
using MyLeasing.Common.Models;
using MyLeasing.Common.Services;
using MyLeasing.Prism.Helpers;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace MyLeasing.Prism.ViewModels
{
    public class EditPropertyViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private PropertyResponse _property;
        private ImageSource _imageSource;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isEdit;
        private bool _isVisible;
        private ObservableCollection<PropertyTypeResponse> _propertyTypes;
        private PropertyTypeResponse _propertyType;
        private ObservableCollection<Stratum> _stratums;
        private Stratum _stratum;
        private MediaFile _file;
        private int _positionImage;
        private DelegateCommand _previousImageCommand;
        private DelegateCommand _nextImageCommand;
        private DelegateCommand _changeImageCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _deleteCommand;
        private DelegateCommand _addImageCommand;
        private DelegateCommand _deleteImageCommand;
        private DelegateCommand _mapCommand;
        public EditPropertyViewModel(
            INavigationService navigationService,
            IApiService apiService
            ) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            LoadTypeProp();
            IsRunning = false;
            IsEnabled = true;
        }
        public DelegateCommand MapCommand => _mapCommand ?? (_mapCommand = new DelegateCommand(Map));

        public DelegateCommand DeleteImageCommand => _deleteImageCommand ?? (_deleteImageCommand = new DelegateCommand(DeleteImage));

        public DelegateCommand PreviousImageCommand => _previousImageCommand ?? (_previousImageCommand = new DelegateCommand(MovePreviousImage));

        public DelegateCommand NextImageCommand => _nextImageCommand ?? (_nextImageCommand = new DelegateCommand(MoveNextImage));

        public DelegateCommand AddImageCommand => _addImageCommand ?? (_addImageCommand = new DelegateCommand(AddImage));

        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImage));

        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(Save));

        public DelegateCommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new DelegateCommand(Delete));

        public ObservableCollection<PropertyTypeResponse> PropertyTypes
        {
            get => _propertyTypes;
            set => SetProperty(ref _propertyTypes, value);
        }

        public TypeProp TypeProp { get; set; }

        public ObservableCollection<TypeProp> TypeProps { get; set; }

        public PropertyTypeResponse PropertyType
        {
            get => _propertyType;
            set => SetProperty(ref _propertyType, value);
        }

        public ObservableCollection<Stratum> Stratums
        {
            get => _stratums;
            set => SetProperty(ref _stratums, value);
        }

        public Stratum Stratum
        {
            get => _stratum;
            set => SetProperty(ref _stratum, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public PropertyResponse Property
        {
            get => _property;
            set => SetProperty(ref _property, value);
        }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }
        private async void Map()
        {
            await _navigationService.NavigateAsync("MainPage");
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("property"))
            {
                Property = parameters.GetValue<PropertyResponse>("property");
                ImageSource = Property.FirstImage;
                IsEdit = true;
                IsVisible = false;
                Title = Languages.EditProperty;
            }
            else
            {
                Title = Languages.NewProperty;
                Property = new PropertyResponse { IsAvailable = true };
                ImageSource = "noImage.png";
                IsEdit = false;
                IsVisible = true;
                Title = Languages.NewProperty;
            }

            LoadPropertyTypes();
            LoadStratums();
        }

        private void LoadStratums()
        {
            Stratums = new ObservableCollection<Stratum>();
            for (int i = 1; i <= 6; i++)
            {
                Stratums.Add(new Stratum { Id = i, Name = $"{i}" });
            }

            Stratum = Stratums.FirstOrDefault(s => s.Id == Property.Stratum);
        }

        private void LoadTypeProp()
        {
            TypeProps = new ObservableCollection<TypeProp>
            {
                new TypeProp { Name = Languages.Sale },
                new TypeProp { Name = Languages.Rent }
            };
        }

        private async void LoadPropertyTypes()
        {
            IsRunning = true;
            IsEnabled = false;

            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var response = await _apiService.GetListAsync<PropertyTypeResponse>(url, "/api", "/PropertyTypes", "bearer", token.Token);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Ok);
                return;
            }

            var propertyTypes = (List<PropertyTypeResponse>)response.Result;
            PropertyTypes = new ObservableCollection<PropertyTypeResponse>(propertyTypes);

            if (!string.IsNullOrEmpty(Property.PropertyType))
            {
                PropertyType = PropertyTypes.FirstOrDefault(pt => pt.Name == Property.PropertyType);
            }
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.PictureSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.FromCamera);

            if (source == Languages.Cancel)
            {
                _file = null;
                return;
            }

            if (source == Languages.FromCamera)
            {
                _file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                _file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (_file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = _file.GetStream();
                    return stream;
                });
            }
        }

        private async void Save()
        {
            IsRunning = true;
            IsEnabled = false;
            var isValid = await ValidateData();
            if (!isValid)
            {
                IsRunning = false;
                IsEnabled = true;
                return;
            }

            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var owner = JsonConvert.DeserializeObject<OwnerResponse>(Settings.Owner);

            var propertyRequest = new PropertyRequest
            {
                Address = Property.Address,
                HasParkingLot = Property.HasParkingLot,
                Id = Property.Id,
                IsAvailable = Property.IsAvailable,
                Neighborhood = Property.Neighborhood,
                OwnerId = owner.Id,
                Price = Property.Price,
                PropertyTypeId = PropertyType.Id,
                Remarks = Property.Remarks,
                Rooms = Property.Rooms,
                Bathrooms = Property.Bathrooms,
                Balconies = Property.Balconies,
                SquareMeters = Property.SquareMeters,
                Stratum = Property.Stratum,
                Latitude = Property.Latitude,
                Longitude = Property.Longitude,
                Typeprop = TypeProp.Name
            };

            Response<object> response;
            if (IsEdit)
            {
                response = await _apiService.PutAsync(url, "/api", "/Properties", propertyRequest.Id, propertyRequest, "bearer", token.Token);
            }
            else
            {
                response = await _apiService.PostAsync(url, "/api", "/Properties", propertyRequest, "bearer", token.Token);
            }

            byte[] imageArray;
            if (_file != null)
            {
                imageArray = FilesHelper.ReadFully(_file.GetStream());
                if (Property.Id == 0)
                {
                    var response2 = await _apiService.GetLastPropertyByOwnerId(url, "/api", "/Properties/GetLastPropertyByOwnerId", "bearer", token.Token, owner.Id);
                    if (response2.IsSuccess)
                    {
                        var property = (PropertyResponse)response2.Result;
                        Property.Id = property.Id;
                    }
                }

                if (Property.Id != 0)
                {
                    var imageRequest = new ImageRequest
                    {
                        PropertyId = Property.Id,
                        ImageArray = imageArray
                    };

                    var response3 = await _apiService.PostAsync(url, "/api", "/Properties/AddImageToProperty", imageRequest, "bearer", token.Token);
                    if (!response3.IsSuccess)
                    {
                        IsRunning = false;
                        IsEnabled = true;
                        await App.Current.MainPage.DisplayAlert(Languages.Error, response3.Message, Languages.Ok);
                    }
                }
            }

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Ok);
                return;
            }

            await PropertiesPageViewModel.GetInstance().UpdateOwner();

            IsRunning = false;
            IsEnabled = true;

            await App.Current.MainPage.DisplayAlert(
                Languages.Accept,
                string.Format(Languages.CreateEditPropertyConfirm, IsEdit ? Languages.Edited : Languages.Created),
                Languages.Ok);

            await _navigationService.GoBackToRootAsync();
        }

        private async Task<bool> ValidateData()
        {
            if (string.IsNullOrEmpty(Property.Neighborhood))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.NeighborhoodError, Languages.Ok);
                return false;
            }

            if (string.IsNullOrEmpty(Property.Address))
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.AddressError, Languages.Ok);
                return false;
            }

            if (Property.Price <= 0)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PriceError, Languages.Ok);
                return false;
            }

            if (Property.SquareMeters <= 0)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.SquareMetersError, Languages.Ok);
                return false;
            }      

            if (Stratum != null)
            {
               Property.Stratum = Stratum.Id;              
            }

            if (PropertyType == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.TypepropError, Languages.Ok);
                return false;
            }
            if (TypeProp == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.PropertyTypeError, Languages.Ok);
                return false;
            }
            if (TypeProp.Name == "Sale")
            {
                TypeProp.Name = "بيع";
            }
            else if (TypeProp.Name == "Rent")
            {
                TypeProp.Name = "استئجار";
            }
            if (Property.Latitude == 0 && Property.Longitude == 0)
            {
                if (Settings.Latitude != "" && Settings.Longitude != "")
                {
                    Property.Latitude = Convert.ToDouble(Settings.Latitude);
                    Property.Longitude = Convert.ToDouble(Settings.Longitude);
                }
                else
                {
                    if (!CrossGeolocator.Current.IsGeolocationEnabled)
                    {
                        await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.GPS, Languages.Ok);
                        return false;
                    }
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;
                    var location = await locator.GetPositionAsync();
                    Property.Latitude = location.Latitude;
                    Property.Longitude = location.Longitude;
                }
            }

            return true;
        }

        private async void Delete()
        {
            var answer = await App.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.QuestionToDeleteProperty,
                Languages.Yes,
                Languages.No);

            if (!answer)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var response = await _apiService.DeleteAsync(url, "/api", "/Properties", Property.Id, "bearer", token.Token);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Ok);
                return;
            }

            await PropertiesPageViewModel.GetInstance().UpdateOwner();

            IsRunning = false;
            IsEnabled = true;
            await App.Current.MainPage.DisplayAlert(Languages.Accept,
                string.Format(Languages.CreateEditPropertyConfirm, Languages.Delete),
                 Languages.Ok);
            await _navigationService.GoBackToRootAsync();
        }


        private async void AddImage()
        {
            if (_file == null)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.AddImageError1, Languages.Ok);
                return;
            }

            if (Property.Id == 0)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.AddImageError2, Languages.Ok);
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var imageArray = FilesHelper.ReadFully(_file.GetStream());
            var imageRequest = new ImageRequest
            {
                PropertyId = Property.Id,
                ImageArray = imageArray
            };

            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var response = await _apiService.PostAsync(url, "/api", "/Properties/AddImageToProperty", imageRequest, "bearer", token.Token);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Ok);
                return;
            }

            await App.Current.MainPage.DisplayAlert(Languages.Accept, Languages.AddImageConfirm, Languages.Ok);
            _file = null;
        }

        private void MoveNextImage()
        {
            MoveImage(1);
        }

        private void MovePreviousImage()
        {
            MoveImage(-1);
        }

        private void MoveImage(int delta)
        {
            if (Property.PropertyImages == null || Property.PropertyImages.Count <= 1)
            {
                return;
            }

            _positionImage += delta;

            if (_positionImage < 0)
            {
                _positionImage = Property.PropertyImages.Count - 1;
            }

            if (_positionImage > Property.PropertyImages.Count - 1)
            {
                _positionImage = 0;
            }

            ImageSource = Property.PropertyImages.ToList()[_positionImage].ImageUrl;
        }

        private async void DeleteImage()
        {
            if (Property.PropertyImages.Count == 0)
            {
                return;
            }

            var answer = await App.Current.MainPage.DisplayAlert(Languages.Confirm, Languages.DeleteImageConfirmation, Languages.Yes, Languages.No);
            if (!answer)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var imageRequest = new ImageRequest
            {
                Id = Property.PropertyImages.ToList()[_positionImage].Id,
                PropertyId = Property.Id,
            };

            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var response = await _apiService.PostAsync(url, "/api", "/Properties/DeleteImageToProperty", imageRequest, "bearer", token.Token);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Ok);
                return;
            }

            await App.Current.MainPage.DisplayAlert(Languages.Accept, Languages.ImageDeletedMessage, Languages.Ok);
            await PropertiesPageViewModel.GetInstance().UpdateOwner();
            await _navigationService.GoBackToRootAsync();
        }
    }
}
