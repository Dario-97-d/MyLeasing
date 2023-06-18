using System.ComponentModel.DataAnnotations;

namespace MyLeasing.Web.Models
{
    public class RegisterNewUserViewModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }


        [Display(Name = "Document")]
        [MaxLength(20, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Document { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Username")]
        public string Username { get; set; }


        [Required]
        [MinLength(8)]
        public string Password { get; set; }


        [Compare("Password")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
