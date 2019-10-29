using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MyLeasing.Web.Data.Entities;

namespace MyLeasing.Web.Models
{
    public class PropertyImageViewModel : PropertyImage
    {
        [Display(Name = "الصور")]
        public IFormFile ImageFile { get; set; }
    }
}
