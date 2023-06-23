using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MyLeasing.Web.Models;

namespace MyLeasing.Web.Data.Entities
{
    public class Owner : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        [DisplayName("Document*")]
        public string Document { get; set; }

        [Required]
        [MaxLength(15)]
        [DisplayName("First Name*")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(15)]
        [DisplayName("Last Name*")]
        public string LastName { get; set; }

        [DisplayName("Full name")]
        public string OwnerName => $"{FirstName} {LastName}";

        [MaxLength(15)]
        [DisplayName("Fixed Phone")]
        public string FixedPhone { get; set; }

        [MaxLength(15)]
        [DisplayName("Cell Phone")]
        public string CellPhone { get; set; }

        [MaxLength(63)]
        public string Address { get; set; }

        public User User { get; set; }

        public string PhotoFullPath => PhotoId == Guid.Empty ?
            "https://myleasing.azurewebsites.net/images/no_image_icon.png" :
            "https://myleasingdariostorage.blob.core.windows.net/owners/" + PhotoId;

        public Guid PhotoId { get; set; }


        public static OwnerViewModel ToOwnerViewModel(Owner owner)
        {
            return new()
            {
                Id = owner.Id,
                Document = owner.Document,
                FirstName = owner.FirstName,
                LastName = owner.LastName,
                FixedPhone = owner.FixedPhone,
                CellPhone = owner.CellPhone,
                Address = owner.Address,
                User = owner.User,
                PhotoId = owner.PhotoId
            };
        }
    }
}
