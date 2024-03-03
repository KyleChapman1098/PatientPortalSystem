using Microsoft.AspNetCore.Mvc;
using PatientPortalSystem.Data;
using PatientPortalSystem.Models;
using Microsoft.AspNetCore.Http;

namespace PatientPortalSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext db;

        public AccountController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User obj)
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
                return RedirectToAction("Login");
            }
            TempData["Error"] = "Account creation failed";
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User obj)
        {
            if (db.DefaultUser.Any(x => x.Username == obj.Username && x.Password == obj.Password))
            {
                var user = db.DefaultUser.FirstOrDefault(x => x.Username == obj.Username);
                
                if(user.Role == "Admin")
                {
                    HttpContext.Session.SetString("IsAdmin", "true");
                    return RedirectToAction("Index", "Admin");
                }
                if(user.Role == "Patient")
                {
                    HttpContext.Session.SetString("IsVerified", "true");
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    
                    return RedirectToAction("Index", "Patient", user);
                }
                if(user.Role == "Doctor")
                {
                    HttpContext.Session.SetString("IsDoctor", "true");
                    HttpContext.Session.SetInt32("UserId", user.Id);

                    return RedirectToAction("Index", "Doctor");
                }
            }
            TempData["Error"] = "Login failed";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Logout Successful";
            return RedirectToAction("Index", "Home");
        }
    }
}
