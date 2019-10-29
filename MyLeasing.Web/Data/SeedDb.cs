using System;
using System.Linq;
using System.Threading.Tasks;
using MyLeasing.Web.Data.Entities;
using MyLeasing.Web.Helpers;

namespace MyLeasing.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(
            DataContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRoles();
            var manager = await CheckUserAsync(123456789, "admin", "admin", "myleasing12@gmail.com", "0597269634", "jenin", "Manager");
            var owner = await CheckUserAsync(589275663, "مرح", "رحال", "Marah.2019h@gmail.com", "0593675683", "جنين", "Owner");
            var lessee = await CheckUserAsync(583691443, "امير", "مقالدة", "ameer@gmail.com", "0568367873", "جنين", "Lessee");
            await CheckPropertyTypesAsync();
            await CheckManagerAsync(manager);
            await CheckOwnersAsync(owner);
            await CheckLesseesAsync(lessee);
            await CheckPropertiesAsync();
        }

       
        private async Task CheckManagerAsync(User user)
        {
            if (!_context.Managers.Any())
            {
                _context.Managers.Add(new Manager { User = user });
                await _context.SaveChangesAsync();
            }
        }

        private async Task<User> CheckUserAsync(
            int document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            string role)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, role);

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }

        private async Task CheckRoles()
        {
            await _userHelper.CheckRoleAsync("Manager");
            await _userHelper.CheckRoleAsync("Owner");
            await _userHelper.CheckRoleAsync("Lessee");
        }

        private async Task CheckLesseesAsync(User user)
        {
            if (!_context.Lessees.Any())
            {
                _context.Lessees.Add(new Lessee { User = user });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckPropertiesAsync()
        {
            var owner = _context.Owners.FirstOrDefault();
            var propertyType = _context.PropertyTypes.FirstOrDefault();
            if (!_context.Properties.Any())
            {
                AddProperty("جنين", "الجابريات", owner, propertyType, 800000M, 2, 72, 4,"بيع");
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckPropertyTypesAsync()
        {
            if (!_context.PropertyTypes.Any())
            {
                _context.PropertyTypes.Add(new PropertyType { Name = "شقة" });
                _context.PropertyTypes.Add(new PropertyType { Name = "منزل" });
                _context.PropertyTypes.Add(new PropertyType { Name = "فيلا" });
                _context.PropertyTypes.Add(new PropertyType { Name = "أرض" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckOwnersAsync(User user)
        {
            if (!_context.Owners.Any())
            {
                _context.Owners.Add(new Owner { User = user });
                await _context.SaveChangesAsync();
            }
        }

        private void AddProperty(string address, string neighborhood, Owner owner, PropertyType propertyType, decimal price, int rooms, int squareMeters, int stratum, string typeprop)
        {
            _context.Properties.Add(new Property
            {
                Address = address,
                HasParkingLot = true,
                IsAvailable = true,
                Neighborhood = neighborhood,
                Owner = owner,
                Price = price,
                PropertyType = propertyType,
                Rooms = rooms,
                SquareMeters = squareMeters,
                Stratum = stratum,
                Typeprop= typeprop,
            });
        }
    }
}
