using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyLeasing.Web.Data.Entities;

namespace MyLeasing.Web.Data.Repository
{
    public class LesseeRepository : GenericRepository<Lessee>, ILesseeRepository
    {
        readonly DataContext _context;

        public LesseeRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Lessee> GetAllWithUsers()
        {
            return _context.Lessees.Include(l => l.User);
        }
    }
}
