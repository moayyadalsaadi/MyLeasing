using MyLeasing.Common.Helpers;
using System.ComponentModel.DataAnnotations;

namespace MyLeasing.Web.Data.Entities
{
    public class PropertyImage
    {
        public int Id { get; set; }

        [Display(Name = "صورة")]
        public string ImageUrl { get; set; }

        // TODO: Change the path when publish
        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl) 
            ? null
            : $"{URL.UrlAPI}{ImageUrl.Substring(1)}";

        public Property Property { get; set; }
    }
}
