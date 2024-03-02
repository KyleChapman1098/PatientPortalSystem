using Microsoft.AspNetCore.Mvc;
using PatientPortalSystem.Models;
using Microsoft.AspNetCore.Http;
using PatientPortalSystem.Data;

namespace PatientPortalSystem.Controllers
{
    public class PatientController : Controller
    {

        private readonly ApplicationDbContext db;

        public PatientController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("IsVerified") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }
            var UserId = HttpContext.Session.GetInt32("UserId");

            var User = db.DefaultUser.FirstOrDefault(x => x.Id == UserId);

            return View(User);
        }

        public IActionResult UpdateAccount()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }
            var UserId = HttpContext.Session.GetInt32("UserId");

            var User = db.DefaultUser.Find(UserId);

            return View(User);
        }

        [HttpPost]
        public IActionResult UpdateAccount(User obj)
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
