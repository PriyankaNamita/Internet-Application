using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HealthCare.Models
{
    public class Prescription
    {
        public int ID { get; set; }
        [ForeignKey("Appointment")]
        public int AppointmentID { get; set; }
        public Appointment Appointment { get; set; }
        public List<PresMed> PresMed { get; set; }
    }
}