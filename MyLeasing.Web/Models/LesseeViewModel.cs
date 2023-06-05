using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using MyLeasing.Web.Data.Entities;

namespace MyLeasing.Web.Models
{
    public class LesseeViewModel : Lessee
    {
        [DisplayName("Upload Photo")]
        public IFormFile PhotoFile { get; set; }
    }
}
