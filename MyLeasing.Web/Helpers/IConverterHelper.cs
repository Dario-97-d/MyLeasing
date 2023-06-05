using MyLeasing.Web.Data.Entities;
using MyLeasing.Web.Models;

namespace MyLeasing.Web.Helpers
{
    public interface IConverterHelper
    {
        Lessee ToLessee(LesseeViewModel lvm, bool isNew);
        LesseeViewModel ToLesseeViewModel(Lessee lessee);
    }
}
