using MyLeasing.Web.Data.Entities;

namespace MyLeasing.Web.Data.Repository
{
    public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
    {
        public OwnerRepository(DataContext context) : base(context)
        {
        }
    }
}
