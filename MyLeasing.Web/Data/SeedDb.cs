using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyLeasing.Web.Data.Entities;
using MyLeasing.Web.Helpers;

namespace MyLeasing.Web.Data
{
    public class SeedDb
    {
        readonly DataContext _context;
        readonly IUserHelper _userHelper;

        readonly string _defaultUserEmail = "dario@e.mail";
        User _defaultUser;


        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }


        /// <summary>
        /// Seeds each database table if empty.
        /// </summary>
        public async Task SeedAsync()
        {
            // Ensure Database is Created
            await _context.Database.EnsureCreatedAsync();

            // Seed items

            await SeedRolesAsync();
            await SeedUserAsync();
            SeedOwners();
            SeedLessees();

            // Save changes to database, if there are any
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Calls UserHelper to create User and add it to Role.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="password">Password for User.</param>
        /// <param name="roleName">Name of the Role to add User to.</param>
        /// <exception cref="Exception">Could not add User.</exception>
        async Task AddUserWithRoleAsync(User user, string password, string roleName)
        {
            var result = await _userHelper.AddUserAsync(_defaultUser, password);

            if (!result.Succeeded)
                throw new Exception(
                    $"Could not add {nameof(User)} at " +
                    $"{nameof(SeedDb)}.{nameof(AddUserWithRoleAsync)}().");

            await _userHelper.AddUserToRoleAsync(_defaultUser, roleName);
        }


        /// <summary>
        /// Defines a default User to seed in database.
        /// </summary>
        /// <returns>Default User.</returns>
        User DefineDefaultUser()
        {
            return new User()
            {
                Email = _defaultUserEmail,
                UserName = _defaultUserEmail,
                Document = "1",
                FirstName = "Dário",
                LastName = "Dias",
                Address = "Rua das Casas"
            };
        }

        /// <summary>
        /// Defines five Lessees to seed in database.
        /// </summary>
        /// <returns>Array of Lessees.</returns>
        Lessee[] DefineFiveLessees()
        {
            Lessee[] lessees = new Lessee[5];

            string[] docs = { "12345", "23456", "34567", "45678", "56789" };

            string[] firstNames = { "Kelvin", "Laney", "Matt", "Neal", "Oliver" };
            string[] lastNames = { "Klein", "Staley", "Hers", "Bill", "Tsubasa" };

            string fixedPhone = "0022446688";
            string cellPhone = "123456789";
            string address = "Avenida das Mansões";

            _defaultUser ??= DefineDefaultUser();

            for (int i = 0; i < 5; i++)
            {
                lessees[i] = new Lessee()
                {
                    Document = docs[i],
                    FirstName = firstNames[i],
                    LastName = lastNames[i],
                    PhotoId = Guid.Empty,
                    FixedPhone = fixedPhone,
                    CellPhone = cellPhone,
                    Address = address,
                    User = _defaultUser
                };
            }

            return lessees;
        }

        /// <summary>
        /// Defines ten Owners.
        /// </summary>
        /// <returns>Owner[].</returns>
        Owner[] DefineTenOwners()
        {
            Owner[] owners = new Owner[10];

            string[] docs =
            {
                "0000000000", "1111111111", "2222222222", "3333333333", "4444444444",
                "5555555555", "6666666666", "7777777777", "8888888888", "9999999999"
            };
            string[] firstNames =
                { "Asdrúbal", "Belinda", "Câncio", "Dora", "Eugénio", "Felisberta", "Germano", "Helga", "Idiota", "Joela" };
            string[] lastNames =
                { "Antonelli", "Bruges", "Covas", "Dias", "Eduardo", "Fuinha", "Germes", "Hufflepuff", "Ioio", "Júlia" };

            string fixedPhone = "0022446688";
            string cellPhone = "987654321";
            string address = "Rua das Casas";

            _defaultUser ??= DefineDefaultUser();

            for (int i = 0; i < 10; i++)
            {
                owners[i] = new Owner()
                {
                    Document = docs[i],
                    FirstName = firstNames[i],
                    LastName = lastNames[i],
                    PhotoId = Guid.Empty,
                    FixedPhone = fixedPhone,
                    CellPhone = cellPhone,
                    Address = address,
                    User = _defaultUser
                };
            }

            return owners;
        }


        /// <summary>
        /// Seeds Lessees in Data Context if there aren't any yet.
        /// </summary>
        /// <returns>true if any change was made; otherwise, false.</returns>
        void SeedLessees()
        {
            // Check any Lessee exists
            if (_context.Lessees.Any())
                return;

            // Define and add lessees
            Lessee[] lessees = DefineFiveLessees();
            _context.Lessees.AddRange(lessees);
        }

        /// <summary>
        /// Seeds Owners in database, if there aren't any yet.
        /// </summary>
        /// <returns>true if seeding has been done; otherwise, false.</returns>
        void SeedOwners()
        {
            // Seed only if there aren't any Owners yet
            if (_context.Owners.Any())
                return;

            // Define and add owners
            Owner[] owners = DefineTenOwners();
            _context.Owners.AddRange(owners);
        }

        /// <summary>
        /// Seeds Roles in database if the default ones don't exist yet.
        /// </summary>
        /// <returns>true if any change was made; otherwise, false.</returns>
        async Task SeedRolesAsync()
        {
            string[] roles = { "Admin", "Owner", "Lessee", "Standard" };

            foreach (string role in roles)
            {
                await _userHelper.EnsureRoleExistsAsync(role);
            }
        }

        /// <summary>
        /// Seeds User in database if the default one doesn't exist.
        /// </summary>
        /// <returns>true if any change was made; otherwise, false.</returns>
        async Task SeedUserAsync()
        {
            // Check user exists
            _defaultUser = await _userHelper.GetUserByEmailAsync(_defaultUserEmail);

            // If not
            if (_defaultUser == null)
            {
                _defaultUser = DefineDefaultUser();
                string password = _defaultUserEmail;

                await AddUserWithRoleAsync(_defaultUser, password, "Admin");

                return;
            }

            // Else

            if (!await _userHelper.IsUserInRoleAsync(_defaultUser, "Admin"))
            {
                await _userHelper.AddUserToRoleAsync(_defaultUser, "Admin");
            }
        }

    }
}
