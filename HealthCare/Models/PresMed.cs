using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HealthCare.Models
{
    public class PresMed
    {
        [Key]
        [ForeignKey("Prescription")]
        [Column("PrescriptionID", Order = 1)]
        public int PrescriptionID { get; set; }
        public Prescription Prescription { get; set; }
        [Key]
        [ForeignKey("Medicine")]
        [Column("MedicineID", Order = 2)]
        public int MedicineID { get; set; }
        public Medicine Medicine { get; set; }
        public string Dosage { get; set; }
    }
}