using HealthCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCare.Repository
{
    public class DoctorRepo
    {
        DatabaseContext _context;
        public DoctorRepo()
        {
            _context = new DatabaseContext();
        }

        public List<Doctor> GetAllDoctors()
        {
            return _context.Doctors.ToList();
        }

        public Doctor GetById(int id)
        {
            return _context.Doctors.FirstOrDefault(d => d.ID == id);
        }
    }
}