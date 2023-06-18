using System.ComponentModel.DataAnnotations;

namespace MyLeasing.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        
        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
