using System.ComponentModel.DataAnnotations;

namespace MyLeasing.Web.Models
{
    public class RecoverPasswordViewModel
    {
        [Display(Name = "البريد الالكتروني*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [EmailAddress( ErrorMessage = " {0} غير صحيح.")]
        public string Email { get; set; }
    }
}
