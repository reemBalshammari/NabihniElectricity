using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NabihniElectricity.Models
{
    public class Report
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المبلغ مطلوب")]
        [Display(Name = "اسم المبلغ")]
        public string ReporterName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الجوال مطلوب")]
        [Display(Name = "رقم الجوال")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "الموقع مطلوب")]
        [Display(Name = "الموقع")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "نوع البلاغ مطلوب")]
        [Display(Name = "نوع البلاغ")]
        public string ReportType { get; set; } = string.Empty;

        [Required(ErrorMessage = "الوصف مطلوب")]
        [Display(Name = "الوصف")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "تاريخ البلاغ")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "الحالة")]
        public string Status { get; set; } = "جديد";

        // اسم الصورة الذي يُحفظ في قاعدة البيانات
        public string? ImagePath { get; set; }

        // الصورة المرفوعة (لا تُحفظ في قاعدة البيانات)
        [Required(ErrorMessage = "صورة البلاغ مطلوبة")]
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string? UserId { get; set; }

public ApplicationUser? User { get; set; }
    }
}