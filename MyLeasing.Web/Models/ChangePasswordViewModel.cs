using System.ComponentModel.DataAnnotations;

namespace MyLeasing.Web.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "كلمة المرور الحالية*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "يجب أن يحتوي الحقل {0} بين {2} و {1} حرفًا.")]
        public string OldPassword { get; set; }

        [Display(Name = "كلمة المرور الجديدة*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "يجب أن يحتوي الحقل {0} بين {2} و {1} حرفًا.")]
        public string NewPassword { get; set; }

        [Display(Name = "تاكيد كلمة المرور*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "يجب أن يحتوي الحقل {0} بين {2} و {1} حرفًا.")]
        [Compare("NewPassword")]
        public string Confirm { get; set; }
    }
}
