using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyLeasing.Web.Data.Entities;

namespace MyLeasing.Web.Models
{
    public class ContractViewModel : Contract
    {
        public int OwnerId { get; set; }

        public int PropertyId { get; set; }

        [Required(ErrorMessage = "الحقل {0} الزامي.")]
        [Display(Name = "مستأجر")]
        [Range(1, int.MaxValue, ErrorMessage = "يجب اختيار المستأجر.")]
        public int LesseeId { get; set; }

        public IEnumerable<SelectListItem> Lessees { get; set; }
    }
}
