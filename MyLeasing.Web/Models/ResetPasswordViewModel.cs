using System.ComponentModel.DataAnnotations;

namespace MyLeasing.Web.Models
{
    public class ResetPasswordViewModel
    {
        [Display(Name = "البريد الالكتروني*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [EmailAddress( ErrorMessage = " {0} غير صحيح.")]
        public string UserName { get; set; }

        [Display(Name = "كلمة المرور*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "يجب أن يحتوي الحقل {0} بين {2} و {1} حرفًا.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تاكيد كلمة المرور*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]       
        [StringLength(20, MinimumLength = 6, ErrorMessage = "يجب أن يحتوي الحقل {0} بين {2} و {1} حرفًا.")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
