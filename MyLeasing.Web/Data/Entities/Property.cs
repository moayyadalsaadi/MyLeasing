﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyLeasing.Web.Data.Entities
{
    public class Property
    {
        public int Id { get; set; }

        [Display(Name = "الحي*")]
        [MaxLength(50, ErrorMessage = "لا يمكن أن يحتوي الحقل {0} على أكثر من {1} حرف.")] 
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string Neighborhood { get; set; }

        [Display(Name = "العنوان*")]
        [MaxLength(50, ErrorMessage = "لا يمكن أن يحتوي الحقل {0} على أكثر من {1} حرف.")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string Address { get; set; }

        [Display(Name = "السعر*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public decimal Price { get; set; }

        [Display(Name = "المساحة*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public int SquareMeters { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "الغرف*")]
        public int Rooms { get; set; }

        [Display(Name = "الطوابق*")]
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public int Stratum { get; set; }

        [Display(Name = "لديها موقف سيارات?")]
        public bool HasParkingLot { get; set; }

        [Display(Name = "متوفر?")]
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "طبيعة العقار*")]
        public string Typeprop { get; set; }

        [Display(Name = "ملاحظات")]
        public string Remarks { get; set; }

        [Display(Name = "خط العرض")]
        [DisplayFormat(DataFormatString = "{0:N6}")]
        public double Latitude { get; set; }

        [Display(Name = "خط الطول")]
        [DisplayFormat(DataFormatString = "{0:N6}")]
        public double Longitude { get; set; }

        public PropertyType PropertyType { get; set; }

        public Owner Owner { get; set; }

        public ICollection<PropertyImage> PropertyImages { get; set; }

        public ICollection<Contract> Contracts { get; set; }

        public string FirstImage
        {
            get
            {
                if (PropertyImages == null || PropertyImages.Count == 0)
                {
                    return "http://localhost:50000/images/Properties/noImage.png";
                }

                return PropertyImages.FirstOrDefault().ImageUrl;
            }
        }
    }
}