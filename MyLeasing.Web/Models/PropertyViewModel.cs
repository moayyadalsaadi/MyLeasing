using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyLeasing.Web.Data.Entities;

namespace MyLeasing.Web.Models
{
    public class PropertyViewModel : Property
    {
        public int OwnerId { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "نوع العقار*")]
        [Range(1, int.MaxValue, ErrorMessage = "يجب اختيار نوع العقار.")]
        public int PropertyTypeId { get; set; }

        public IEnumerable<SelectListItem> PropertyTypes { get; set; }
    }
}
