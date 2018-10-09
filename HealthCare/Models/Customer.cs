using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCare.Models
{
    public class Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Decimal Weight { get; set; }
        public Decimal Age { get; set; }
        public Boolean IsActive { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<Report> Reports { get; set; }
    }
}