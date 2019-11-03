using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyLeasing.Web.Models
{
    public class AddUserViewModel : EditUserViewModel
    {
        [Display(Name = "البريد الالكتروني*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [MaxLength(100, ErrorMessage = "لا يمكن أن يحتوي الحقل {0} على أكثر من {1} حرف.")]
        [EmailAddress( ErrorMessage = " {0} غير صحيح.")]
        public string Username { get; set; }

        [Display(Name = "كلمة المرور*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "يجب أن يحتوي الحقل {0} بين {2} و {1} حرفًا.")]
        public string Password { get; set; }

        [Display(Name = "تاكيد كلمة المرور*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "يجب أن يحتوي الحقل {0} بين {2} و {1} حرفًا.")]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "صلاحية كـ*")]
        [Range(1, int.MaxValue, ErrorMessage = "يجب اختيار الصلاحية.")]
        public int RoleId { get; set; }

        [Display(Name = "كود التحقق*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [StringLength(6, ErrorMessage = "يجب أن يحتوي الحقل {0} على {1} حروف.")]
        public string CaptchaCode { get; set; }
        
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
