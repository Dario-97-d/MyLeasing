using System.ComponentModel.DataAnnotations;

namespace MyLeasing.Web.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
