using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        [DisplayName("Owner Name")]
        public string OwnerName => $"{FirstName} {LastName}";

        [MaxLength(15)]
        [DisplayName("Fixed Phone")]
        public string FixedPhone { get; set; }

        [MaxLength(15)]
        [DisplayName("Cell Phone")]
        public string CellPhone { get; set; }

        [MaxLength(63)]
        public string Address { get; set; }
    }
}
