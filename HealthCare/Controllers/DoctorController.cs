using HealthCare.Models;
using HealthCare.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthCare.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        DatabaseContext databaseContext;
        ApplicationDbContext context;
        DoctorRepo _doctorRepo;
        public DoctorController()
        {
            databaseContext = new DatabaseContext();
            context = new ApplicationDbContext();
            _doctorRepo = new DoctorRepo();

        }
        // GET: Doctor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Appointment()
        {
            int doctorId = GetDoctorId();
            var dateNow = DateTime.Now;
            List<Appointment> apps = new List<Appointment>();
            var appointments = databaseContext.Appointments.Include("Customer").Where(a => a.DoctorID == doctorId).ToList();
            foreach (var app in appointments)
            {
               var result = DateTime.Compare(dateNow.Date, app.OnDate.Date);
                if (result==0)
                {
                    apps.Add(app);

                }
            }

            return View(apps);
        }

        [HttpGet]
        public ActionResult Prescription(int id)
        {
            ViewBag.Name = new SelectList(databaseContext.Medicines.ToList(), "ID", "Name");
            ViewBag.ID = id;
            return View();
        }

        [HttpPost]
        public JsonResult Prescription(Prescription prescription, int id)
        {

            databaseContext.Prescriptions.Add(prescription);
            databaseContext.SaveChanges();
            return Json(prescription, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult UploadReport(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadReport(Report report,int id)
        {
            try
            {
                var repo = new Report();
                repo.CustomerID = id;
                repo.OnDate = DateTime.Now;
                repo.Path = SaveToPhysicalLocation(report.ReportFile);
                databaseContext.Reports.Add(repo);
                databaseContext.SaveChanges();
                return RedirectToAction("AllAppointment");
            }
            catch
            {
                return View();
            }


        }

        [HttpGet]
        public ActionResult AllAppointment()
        {
            int doctorId = GetDoctorId();
  
            var appointments = databaseContext.Appointments.Include("Customer").Where(a => a.DoctorID == doctorId).ToList();


            return View(appointments);
        }
        private string SaveToPhysicalLocation(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                file.SaveAs(path);
                return path;
            }
            return string.Empty;

        }

            private int GetDoctorId()
        {
            var doctorAuthId = User.Identity.GetUserId();
            var doctorEmail = context.Users.FirstOrDefault(u => u.Id == doctorAuthId).Email;
            return databaseContext.Doctors.FirstOrDefault(d => d.Email == doctorEmail ).ID;
        }
    }
}