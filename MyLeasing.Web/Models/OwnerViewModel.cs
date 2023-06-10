using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using MyLeasing.Web.Data.Entities;

namespace MyLeasing.Web.Models
{
    public class OwnerViewModel : Owner
    {
        [DisplayName("Photo")]
        public IFormFile PhotoFile { get; set; }

        public static Owner ToOwner(OwnerViewModel ovm)
        {
            return new Owner
            {
                Id = ovm.Id,
                Document = ovm.Document,
                FirstName = ovm.FirstName,
                LastName = ovm.LastName,
                FixedPhone = ovm.FixedPhone,
                CellPhone = ovm.CellPhone,
                Address = ovm.Address,
                User = ovm.User,
                PhotoId = ovm.PhotoId
            };
        }
    }
}
