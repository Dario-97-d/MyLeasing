using System.Linq;
using MyLeasing.Web.Data.Entities;

namespace MyLeasing.Web.Data.Repository
{
    public interface ILesseeRepository : IGenericRepository<Lessee>
    {
        IQueryable<Lessee> GetAllWithUsers();
    }
}
