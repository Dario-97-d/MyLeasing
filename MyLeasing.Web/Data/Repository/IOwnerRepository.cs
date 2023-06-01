using System.Collections.Generic;
using System.Threading.Tasks;
using MyLeasing.Web.Data.Entities;

namespace MyLeasing.Web.Data.Repository
{
    public interface IOwnerRepository
    {
        Task<IEnumerable<Owner>> GetOwners();

        Task<Owner> GetOwner(int id);

        Task AddOwner(Owner owner);

        Task UpdateOwner(Owner owner);

        Task RemoveOwner(Owner owner);

        Task<bool> SaveAllAsync();

        Task<bool> OwnersExistsAsync(int id);
    }
}
