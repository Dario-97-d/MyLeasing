using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyLeasing.Web.Data.Entities;

namespace MyLeasing.Web.Data.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Owner>> GetOwners()
        {
            return await _context.Owners.ToListAsync();
        }

        public async Task<Owner> GetOwner(int id)
        {
            return await _context.Owners.FindAsync(id);
        }

        public async Task AddOwner(Owner owner)
        {
            _context.Owners.Add(owner);
            await SaveAllAsync();
        }

        public async Task UpdateOwner(Owner owner)
        {
            _context.Owners.Update(owner);
            await SaveAllAsync();
        }

        public async Task RemoveOwner(Owner owner)
        {
            _context.Owners.Remove(owner);
            await SaveAllAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> OwnersExistsAsync(int id)
        {
            return await _context.Owners.AnyAsync(o => o.Id == id);
        }
    }
}
