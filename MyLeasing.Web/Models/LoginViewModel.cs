using System.ComponentModel.DataAnnotations;

namespace MyLeasing.Web.Models
{
    public class LoginViewModel
    {
        
        [Display(Name = "البريد الالكتروني*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [EmailAddress( ErrorMessage = " {0} غير صحيح.")]
        public string Username { get; set; }

        [Display(Name = "كلمة المرور*")]  
        [StringLength(20, MinimumLength = 6, ErrorMessage = "يجب أن يحتوي الحقل {0} بين {2} و {1} حرفًا.")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string Password { get; set; }

        [Display(Name = "تذكرني؟")]
        public bool RememberMe { get; set; }
    }
}
