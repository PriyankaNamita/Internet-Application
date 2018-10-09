using HealthCare.Models;
using HealthCare.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HealthCare.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private DatabaseContext _databaseContext;
        private ApplicationUserManager _userManager;
        private DoctorRepo _doctorRepo;
        public AdminController()
        {
            _databaseContext = new DatabaseContext();
            _doctorRepo = new DoctorRepo();
        }
        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RegisterDoc()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterDoc(RegisterDocModel model)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (ModelState.IsValid)
            {

                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Doctor";
                //roleManager.Create(role);
             
                var user = new ApplicationUser();
                user.UserName = model.Email;
                user.Email = model.Email;
                var doctor = new Doctor()
                {
                    Name = model.Name,
                    Specialization = model.Specialization,
                    Capacity = model.Capacity,
                    IsActive = true,
                    Email = model.Email
                };

                string userPWD = model.Password;

                var chkUser = UserManager.Create(user, userPWD);    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, role.Name);
                    _databaseContext.Doctors.Add(doctor);
                    _databaseContext.SaveChanges();
                    return RedirectToAction("Index", "Admin");
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public ActionResult ViewDoc()
        {
            return View(_doctorRepo.GetAllDoctors());
        }

        [HttpGet]
        public ActionResult ViewReviews()
        {
            return View(_databaseContext.Reviews.Include("Customer").Include("Doctor").ToList());
        }

        [HttpGet]
        public ActionResult ViewCustomer()
        {
            return View(_databaseContext.Customers.ToList());
        }

        public ActionResult Suspend(int id)
        {
            var customer = _databaseContext.Customers.FirstOrDefault(c => c.ID == id);
            if (customer.IsActive)
            {
                customer.IsActive = false;
            }
            else
            {
                customer.IsActive = true;
            }
            _databaseContext.SaveChanges();
            return RedirectToAction("ViewCustomer");
        }

        [HttpGet]
        public ActionResult Archive(int id)
        {
            var review = _databaseContext.Reviews.FirstOrDefault(r => r.ID == id);
            if (review.IsArchive)
            {
                review.IsArchive = false;
            }
            else
            {
                review.IsArchive = true;
            }
            _databaseContext.SaveChanges();
            return RedirectToAction("ViewReviews");
        }
    }
}