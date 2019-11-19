using MyLeasing.Prism.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyLeasing.Prism.Helpers
{
    public class Languages
    {
 
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept => Resource.Accept;
        public static string Property => Resource.Property;
        public static string Details => Resource.Details;
        public static string Properties1 => Resource.Properties1;
        public static string Properties => Resource.Properties;
        public static string Register1 => Resource.Register1;
        public static string connection => Resource.connection;
        public static string Address => Resource.Address;
        public static string AddressError => Resource.AddressError;
        public static string AddressPlaceHolder => Resource.AddressPlaceHolder;
        public static string Document => Resource.Document;
        public static string DocumentError => Resource.DocumentError;
        public static string DocumentPlaceHolder => Resource.DocumentPlaceHolder;
        public static string Email => Resource.Email;
        public static string EmailError => Resource.EmailError;
        public static string EmailPlaceHolder => Resource.EmailPlaceHolder;
        public static string Error => Resource.Error;
        public static string FirstName => Resource.FirstName;
        public static string FirstNameError => Resource.FirstNameError;
        public static string FirstNamePlaceHolder => Resource.FirstNamePlaceHolder;
        public static string Forgot => Resource.Forgot;
        public static string LastName => Resource.LastName;
        public static string LastNameError => Resource.LastNameError;
        public static string LastNamePlaceHolder => Resource.LastNamePlaceHolder;
        public static string Lessee => Resource.Lessee;
        public static string Login => Resource.Login;
        public static string LoginError => Resource.LoginError;
        public static string Password => Resource.Password;
        public static string PasswordRecover => Resource.PasswordRecover;
        public static string Ok => Resource.Ok;
        public static string Owner => Resource.Owner;
        public static string PasswordError => Resource.PasswordError;
        public static string PasswordError2 => Resource.PasswordError2;
        public static string PasswordError3 => Resource.PasswordError3;
        public static string PasswordPlaceHolder => Resource.PasswordPlaceHolder;
        public static string PasswordConfirm => Resource.PasswordConfirm;
        public static string PasswordConfirmError => Resource.PasswordConfirmError;
        public static string PasswordConfirmPlaceHolder => Resource.PasswordConfirmPlaceHolder;
        public static string Phone => Resource.Phone;
        public static string PhoneError => Resource.PhoneError;
        public static string PhonePlaceHolder => Resource.PhonePlaceHolder;
        public static string Register => Resource.Register;
        public static string RegisterAs => Resource.RegisterAs;
        public static string RegisterAsError => Resource.RegisterAsError;
        public static string RegisterAsTitle => Resource.RegisterAsTitle;
        public static string Rememberme => Resource.Rememberme;
        public static string Logout => Resource.Logout;
        public static string Map => Resource.Map;
        public static string MyProperties => Resource.MyProperties;
        public static string MyContracts => Resource.MyContracts;
        public static string MyProfile => Resource.MyProfile;
        public static string Save => Resource.Save;
        public static string ChangePassword => Resource.ChangePassword;
        public static string UserUpdated => Resource.UserUpdated;
        public static string ConfirmNewPassword => Resource.ConfirmNewPassword;
        public static string ConfirmNewPasswordError => Resource.ConfirmNewPasswordError;
        public static string ConfirmNewPasswordPlaceHolder => Resource.ConfirmNewPasswordPlaceHolder;
        public static string CurrentPassword => Resource.CurrentPassword;
        public static string CurrentPasswordError => Resource.CurrentPasswordError;
        public static string CurrentPasswordPlaceHolder => Resource.CurrentPasswordPlaceHolder;
        public static string NewPassword => Resource.NewPassword;
        public static string NewPasswordError => Resource.NewPasswordError;
        public static string NewPasswordPlaceHolder => Resource.NewPasswordPlaceHolder;
        public static string ErrorNoOwner => Resource.ErrorNoOwner;
        public static string NewProperty => Resource.NewProperty;
        public static string Delete => Resource.Delete;
        public static string EditProperty => Resource.EditProperty;
        public static string ChangeImage => Resource.ChangeImage;
        public static string Neighborhood => Resource.Neighborhood;
        public static string NeighborhoodError => Resource.NeighborhoodError;
        public static string NeighborhoodPlaceHolder => Resource.NeighborhoodPlaceHolder;
        public static string Price => Resource.Price;
        public static string PriceError => Resource.PriceError;
        public static string PricePlaceHolder => Resource.PricePlaceHolder;
        public static string SquareMeters => Resource.SquareMeters;
        public static string SquareMetersError => Resource.SquareMetersError;
        public static string SquareMetersPlaceHolder => Resource.SquareMetersPlaceHolder;
        public static string Rooms => Resource.Rooms;
        public static string RoomsError => Resource.RoomsError;
        public static string RoomsPlaceHolder => Resource.RoomsPlaceHolder;
        public static string Stratum => Resource.Stratum;
        public static string StratumError => Resource.StratumError;
        public static string StratumPlaceHolder => Resource.StratumPlaceHolder;
        public static string PropertyType => Resource.PropertyType;
        public static string PropertyTypeError => Resource.PropertyTypeError;
        public static string PropertyTypePlaceHolder => Resource.PropertyTypePlaceHolder;
        public static string HasParkingLot => Resource.HasParkingLot;
        public static string IsAvailable => Resource.IsAvailable;
        public static string Remarks => Resource.Remarks;
        public static string Contracts => Resource.Contracts;
        public static string AddImage => Resource.AddImage;
        public static string EditImage => Resource.EditImage;
        public static string DeleteImage => Resource.DeleteImage;
        public static string PictureSource => Resource.PictureSource;
        public static string Cancel => Resource.Cancel;
        public static string FromCamera => Resource.FromCamera;
        public static string FromGallery => Resource.FromGallery;
        public static string CreateEditPropertyConfirm => Resource.CreateEditPropertyConfirm;
        public static string Created => Resource.Created;
        public static string Edited => Resource.Edited;
        public static string Confirm => Resource.Confirm;
        public static string QuestionToDeleteProperty => Resource.QuestionToDeleteProperty;
        public static string Yes => Resource.Yes;
        public static string No => Resource.No;
        public static string AddImageError1 => Resource.AddImageError1;
        public static string AddImageError2 => Resource.AddImageError2;
        public static string AddImageConfirm => Resource.AddImageConfirm;
        public static string DeleteImageConfirmation => Resource.DeleteImageConfirmation;
        public static string ImageDeletedMessage => Resource.ImageDeletedMessage;
        public static string Support => Resource.Support;
        public static string Message => Resource.Message;
        public static string Message1 => Resource.Message1;
        public static string Message2 => Resource.Message2;
        public static string Message3 => Resource.Message3;
        public static string NotAddressFound => Resource.NotAddressFound;
        public static string NotLocationAvailable => Resource.NotLocationAvailable;
        public static string SelectAnAddress => Resource.SelectAnAddress;

        public static string GPS => Resource.GPS;

        public static string TypepropError => Resource.TypePropError;

        public static string Sale => Resource.Sale;
        public static string Rent => Resource.Rent;
    }
}
