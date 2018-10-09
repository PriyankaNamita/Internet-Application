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
    [Authorize(Roles = "Customer")]
    public class HomeController : Controller
    {
        DoctorRepo _doctorRepo;
        CustomerRepo _customerRepo;
        DatabaseContext databaseContext;
        ApplicationDbContext context;
        public HomeController()
        {
            _doctorRepo = new DoctorRepo();
            _customerRepo = new CustomerRepo();
            context = new ApplicationDbContext();
            databaseContext = new DatabaseContext();
        }
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Contact()
        {

            return View();
        }

        public ActionResult ViewDoc()
        {

            return View(_doctorRepo.GetAllDoctors());
        }

        [HttpGet]
        public ActionResult BookAppointment(string id)
        {
            var doctor = _doctorRepo.GetById(Convert.ToInt32(id));
            var appointment = new AppointmentViewModel();
            appointment.DoctorID = doctor.ID;
            appointment.DoctorName = doctor.Name;
             
            return View(appointment);
        }

        [HttpPost]
        public ActionResult BookAppointment(AppointmentViewModel appointmentViewModel,int id)
        {
            if (!ModelState.IsValid)
            {
                return View(new { Id = id });
            }

            var customerId = GetCustomerId();
            var customerApps = databaseContext.Appointments.Where(a => a.CustomerID == customerId).ToList();
            foreach (var app in customerApps)
            {
                if (app.OnDate.Date == appointmentViewModel.OnDate.Date && app.DoctorID == appointmentViewModel.DoctorID)
                {
                    return RedirectToAction("CannotBook");
                }
            }
            var appointment = new Appointment()
            {
                CustomerID = customerId,
                DoctorID = appointmentViewModel.DoctorID,
                OnDate = appointmentViewModel.OnDate
            };
            
            try
            {
                var capacity = databaseContext.Doctors.FirstOrDefault(d=>d.ID == appointment.DoctorID).Capacity;
                var capacityBooked = databaseContext.Appointments.Where(a => a.DoctorID == appointment.DoctorID && a.OnDate.Equals(appointment.OnDate)).ToList().Count();
                if (capacity == capacityBooked)
                {
                    return RedirectToAction("NoSeat",new { id = appointment.DoctorID});
                }
                databaseContext.Appointments.Add(appointment);
                databaseContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(appointmentViewModel);
            }
            
        }

        [HttpGet]
        public ActionResult NoSeat(string id)
        {
            ViewBag.Id = id;

            return View();
        }

        public ActionResult Appointment()
        {
            int customerId = GetCustomerId();
            return View(databaseContext.Appointments.Include("Doctor").Where(a=> a.CustomerID == customerId).ToList());
        }

        public ActionResult Prescription(int id)
        {
            var pres = databaseContext.Prescriptions.FirstOrDefault(p => p.AppointmentID == id);
            
            try
            {
                var presMed = databaseContext.PresMeds.Include("Medicine").Where(p => p.PrescriptionID == pres.ID).ToList();
                
                if (pres != null)
                {
                    return View(pres);
                }
            }
            catch
            {
                return RedirectToAction("NoPrescription");

            }

            return RedirectToAction("Appointment");
        }
        public ActionResult NoPrescription()
        {
            return View();
        }
        [HttpGet]
        public ActionResult WriteReview(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult WriteReview(Review review,int id)
        {
            if (!ModelState.IsValid)
            {
                return View(new { Id = id});
            }
            review.DoctorID = id;
            review.CustomerID = GetCustomerId();
            review.OnDate = DateTime.Now;
            review.IsArchive = false;
            databaseContext.Reviews.Add(review);
            databaseContext.SaveChanges();
            return RedirectToAction("ViewDoc");
        }

        [HttpGet]
        public ActionResult ViewReview(int id)
        {
           return View(databaseContext.Reviews.Include("Doctor").Include("Customer").Where(r => r.DoctorID == id).ToList());
        }

        [HttpGet]
        public ActionResult DownloadReport()
        {
            var custId = GetCustomerId();
            return View(databaseContext.Reports.Where(r => r.CustomerID == custId));
        }
        [HttpGet]
        public FilePathResult DownloadExampleFiles(string fileName)
        {
            return new FilePathResult(Request.QueryString["fileName"], "application/pdf");
        }

        public ActionResult CannotBook()
        {
            return View();
        }

        private int GetCustomerId()
        {
            var customerAuthId = User.Identity.GetUserId();
            var customerEmail = context.Users.FirstOrDefault(u => u.Id == customerAuthId).Email;
            return _customerRepo.getByEmail(customerEmail).ID;
        }
    }
}