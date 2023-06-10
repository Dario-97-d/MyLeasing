using System;
using System.ComponentModel.DataAnnotations;
using MyLeasing.Web.Models;

namespace MyLeasing.Web.Data.Entities
{
    public class Lessee : IEntity
    {
        // IEntity
        public int Id { get; set; }

        // Lessees

        [Display(Name = "Document")]
        [MaxLength(20, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Document { get; set; }


        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";


        [Display(Name = "Fixed Phone")]
        [MaxLength(15, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string FixedPhone { get; set; }


        [Display(Name = "Cell Phone")]
        [MaxLength(15, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string CellPhone { get; set; }


        [MaxLength(100, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Address { get; set; }


        public User User { get; set; }


        public Guid PhotoId { get; set; }

        public string PhotoFullPath => PhotoId == Guid.Empty ?
            "https://myleasing.azurewebsites.net/images/no_image_icon.png" :
            "https://myleasingdariostorage.blob.core.windows.net/lessees/" + PhotoId;


        public Lessee()
        {
        }

        public Lessee(LesseeViewModel lvm)
        {
            Id = lvm.Id;
            Document = lvm.Document;
            FirstName = lvm.FirstName;
            LastName = lvm.LastName;
            PhotoId = lvm.PhotoId;
            FixedPhone = lvm.FixedPhone;
            CellPhone = lvm.CellPhone;
            Address = lvm.Address;
            User = lvm.User;
        }

    }
}
