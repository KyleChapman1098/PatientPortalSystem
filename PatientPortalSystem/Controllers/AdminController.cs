using Microsoft.AspNetCore.Mvc;
using PatientPortalSystem.Data;
using PatientPortalSystem.Models;

namespace PatientPortalSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db;
        public AdminController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateStaffAccount() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateStaffAccount(User obj)
        {
            if (db.DefaultUser.Any(x => x.Username == obj.Username))
            {
                ModelState.AddModelError("Username", "An account with this username already exists");
            }
            if (db.DefaultUser.Any(x => x.Email == obj.Email))
            {
                ModelState.AddModelError("Email", "An account with this email already exists");
            }


            if (ModelState.IsValid)
            {
                db.DefaultUser.Add(obj);
                db.SaveChanges();

                var user = db.DefaultUser.FirstOrDefault(x => x.Id == obj.Id);

                Staff newStaff = new Staff();
                newStaff.Id = user.Id;

                db.Add(newStaff);
                db.SaveChanges();

                TempData["Success"] = "Account registered successfully";
				return RedirectToAction("Index");
            }
            TempData["Error"] = "Account creation failed";
            return View();
        }
    }
}
