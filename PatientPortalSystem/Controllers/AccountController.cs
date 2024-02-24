using Microsoft.AspNetCore.Mvc;
using PatientPortalSystem.Data;
using PatientPortalSystem.Models;

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

                return RedirectToAction("Login");
            }
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
                    return RedirectToAction("Index", "Admin");
                }
                if(user.Role == "Patient")
                {
                    return RedirectToAction("Index", "Patient");
                }
            }
            return View();
        }
    }
}
