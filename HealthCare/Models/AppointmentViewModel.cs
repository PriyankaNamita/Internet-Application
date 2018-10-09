using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCare.Models
{
    public class AppointmentViewModel
    {
        public int DoctorID { get; set; }
        public DateTime OnDate { get; set; }
        public string DoctorName { get; set; }
    }
}