using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCare.Models
{
    public class Doctor
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
        public List<Appointment> Appointments{ get; set; }
    }
}