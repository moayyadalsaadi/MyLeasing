using System.ComponentModel.DataAnnotations;

namespace MyLeasing.Web.Models
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "رقم الهوية*")]
        [Range(99999999, 999999999, ErrorMessage = "الرجاء التاكد من رقم الهوية")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public int Document { get; set; }

        [Display(Name = "الاسم الاول*")]
        [MaxLength(50, ErrorMessage = "لا يمكن أن يحتوي الحقل {0} على أكثر من {1} حرف.")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string FirstName { get; set; }

        [Display(Name = "الاسم الاخير*")]
        [MaxLength(50, ErrorMessage = "لا يمكن أن يحتوي الحقل {0} على أكثر من {1} حرف.")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string LastName { get; set; }

        [Display(Name = "العنوان*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [MaxLength(100, ErrorMessage = "لا يمكن أن يحتوي الحقل {0} على أكثر من {1} حرف.")]
        public string Address { get; set; }

        [Display(Name = "رقم الهاتف*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Range(056000000, 0599999999, ErrorMessage = "رقم الجوال المدخل خاطئ")]
        public string PhoneNumber { get; set; }

    }
}
