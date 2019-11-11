using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLeasing.Web.Data.Entities
{
    public class PropertyType
    {
        public int Id { get; set; }

        [Display(Name = "نوع العقار*")]
        [MaxLength(50, ErrorMessage = "لا يمكن أن يحتوي الحقل {0} على أكثر من {1} حرف.")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string Name { get; set; }

        public ICollection<Property> Properties  { get; set; }
    }
}
