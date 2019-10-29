using System.ComponentModel.DataAnnotations;

namespace MyLeasing.Web.Models
{
    public class RecoverPasswordViewModel
    {
        [Display(Name = "البريد الالكتروني*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
