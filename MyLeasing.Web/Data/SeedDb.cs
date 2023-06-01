using MyLeasing.Web.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyLeasing.Web.Data
{
    public class SeedDb
    {
        readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
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
            changes = SeedOwners();

            // Save changes to database, if there are any
            if (changes) await _context.SaveChangesAsync();
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

        /// <summary>
        /// Defines ten Owners.
        /// </summary>
        /// <returns>Owner[].</returns>
        static Owner[] DefineTenOwners()
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

            for (int i = 0; i < 10; i++)
            {
                owners[i] = new Owner()
                {
                    Document = docs[i],
                    FirstName = firstNames[i],
                    LastName = lastNames[i],
                    FixedPhone = fixedPhone,
                    CellPhone = cellPhone,
                    Address = address
                };
            }

            return owners;
        }
    }
}
