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
            if(HttpContext.Session.GetString("IsAdmin") != "true")
            {
                TempData["Error"] = "You must be an admin to access this page";
                return RedirectToAction("Index", "Home");
            }
            IEnumerable<User> objUserList = db.DefaultUser;
            return View(objUserList);
        }

        public IActionResult CreateStaffAccount() 
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                TempData["Error"] = "You must be an admin to access this page";
                return RedirectToAction("Account", "Login");
            }
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

                TempData["Success"] = "Account registered successfully";
				return RedirectToAction("Index");
            }
            TempData["Error"] = "Account creation failed";
            return View();
        }

        public IActionResult UpdateStaffAccount(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var user = db.DefaultUser.Find(id);
            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateStaffAccount(User obj)
        {
            if (db.DefaultUser.Any(x => x.Username == obj.Username && x.Id != obj.Id))
            {
                ModelState.AddModelError("Username", "An account with this username already exists");
            }
            if (db.DefaultUser.Any(x => x.Email == obj.Email && x.Id != obj.Id))
            {
                ModelState.AddModelError("Email", "An account with this email already exists");
            }

            if (ModelState.IsValid)
            {
                db.DefaultUser.Update(obj);
                db.SaveChanges();

                TempData["Success"] = "Account updated successfully";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Account update failed";
            return View();
        }
    }
}
