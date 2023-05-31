using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyLeasing.Web.Data.Entities
{
    public class Owner
    {
        public int Id { get; set; }
        
        [Required]
        [DisplayName("Document*")]
        public string Document { get; set; }

        [Required]
        [DisplayName("First Name*")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name*")]
        public string LastName { get; set; }

        [DisplayName("Owner Name")]
        public string OwnerName => $"{FirstName} {LastName}";

        [DisplayName("Fixed Phone")]
        public string FixedPhone { get; set; }

        [DisplayName("Cell Phone")]
        public string CellPhone { get; set; }

        public string Address { get; set; }
    }
}
