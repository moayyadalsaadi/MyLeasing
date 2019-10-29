using MyLeasing.Common.Models;
using MyLeasing.Prism.Helpers;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyLeasing.Prism.ViewModels
{
    public class LeasingMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public LeasingMasterDetailPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private void LoadMenus()
        {
            var menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_home.png",
                    PageName = "PropertiesPage",
                    Title = Languages.MyProperties
                },

                new Menu
                {
                    Icon = "ic_map.png",
                    PageName = "MapPage",
                    Title = Languages.Map

                },

                new Menu
                {
                    Icon = "ic_person.png",
                    PageName = "Profile",
                   Title = Languages.MyProfile

                },

                new Menu
                {
                    Icon = "ic_exit_to_app.png",
                    PageName = "LoginPage",
                    Title = Languages.Logout
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }
    }
}
