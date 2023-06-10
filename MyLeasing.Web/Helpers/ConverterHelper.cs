using MyLeasing.Web.Data.Entities;
using MyLeasing.Web.Models;

namespace MyLeasing.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Lessee ToLessee(LesseeViewModel lvm, bool isNew)
        {
            return new()
            {
                Id = isNew ? 0 : lvm.Id,
                Document = lvm.Document,
                FirstName = lvm.FirstName,
                LastName = lvm.LastName,
                PhotoId = lvm.PhotoId,
                FixedPhone = lvm.FixedPhone,
                CellPhone = lvm.CellPhone,
                Address = lvm.Address,
                User = lvm.User
            };
        }

        public LesseeViewModel ToLesseeViewModel(Lessee lessee)
        {
            return new()
            {
                Id = lessee.Id,
                Document = lessee.Document,
                FirstName = lessee.FirstName,
                LastName = lessee.LastName,
                PhotoId = lessee.PhotoId,
                FixedPhone = lessee.FixedPhone,
                CellPhone = lessee.CellPhone,
                Address = lessee.Address,
                User = lessee.User
            };
        }
    }
}
