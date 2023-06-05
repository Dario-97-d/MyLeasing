using System.Linq;
using MyLeasing.Web.Data.Entities;

namespace MyLeasing.Web.Data.Repository
{
    public interface IOwnerRepository : IGenericRepository<Owner>
    {
        IQueryable GetAllWithUsers();
    }
}
