using System.Linq;
using System.Threading.Tasks;

namespace MyLeasing.Web.Data.Repository
{
    public interface IGenericRepository<T>
    {
        Task CreateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(int id);
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        Task<bool> SaveAllAsync();
        Task UpdateAsync(T entity);
    }
}