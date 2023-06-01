using System;
using System.Linq;
using System.Threading.Tasks;
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
        /// Seeds database tables, if empty.
        /// </summary>
        public async Task SeedAsync()
        {
            bool changes;

            // Ensure Database is Created
            await _context.Database.EnsureCreatedAsync();

            // Seed
            changes = await SeedUser() | SeedOwners();

            // Save changes to database, if there are any
            if (changes) await _context.SaveChangesAsync();
        }


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
                    FixedPhone = fixedPhone,
                    CellPhone = cellPhone,
                    Address = address,
                    User = _defaultUser
                };
            }

            return owners;
        }

        /// <summary>
        /// Seeds Owners in database, if there aren't any yet.
        /// </summary>
        /// <returns>true if seeding has been done; otherwise, false.</returns>
        bool SeedOwners()
        {
            // Seed only if there aren't any Owners yet
            if (_context.Owners.Any())
                return false;

            // Define and add owners
            Owner[] owners = DefineTenOwners();
            _context.Owners.AddRange(owners);

            return true;
        }

        async Task<bool> SeedUser()
        {
            _defaultUser = await _userHelper.GetUserByEmailAsync(_defaultUserEmail);
            if (_defaultUser == null)
            {
                _defaultUser = DefineDefaultUser();
                var password = "password";

                var result = await _userHelper.AddUserAsync(_defaultUser, password);

                if (!result.Succeeded)
                    throw new Exception("Could not create User at SeedDb.SeedUser().");

                return result.Succeeded;
            }

            return false;
        }

    }
}
