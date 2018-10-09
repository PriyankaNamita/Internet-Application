using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HealthCare.Models
{
    public class Report
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public string Path { get; set; }
        [NotMapped]
        public HttpPostedFileBase ReportFile { get; set; }
        public DateTime OnDate { get; set; }
    }
}