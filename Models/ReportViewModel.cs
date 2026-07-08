using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace NabihniElectricity.Models
{
    public class ReportViewModel
    {
        // بيانات البلاغ الجديد
        public Report NewReport { get; set; } = new Report();

        // قائمة البلاغات
        public List<Report> Reports { get; set; } = new List<Report>();

    }
}