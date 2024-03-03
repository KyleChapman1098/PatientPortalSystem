using Microsoft.AspNetCore.Mvc;
using PatientPortalSystem.Models;
using Microsoft.AspNetCore.Http;
using PatientPortalSystem.Data;
using Microsoft.IdentityModel.Tokens;

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

        public IActionResult Messenger()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }

            IEnumerable<InternalMessage> messageList = db.InternalMessaging.Where(x => x.SenderId == HttpContext.Session.GetInt32("UserId") || x.ReceiverId == HttpContext.Session.GetInt32("UserId"));
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View(messageList);
        }

        public IActionResult SendMessage()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }
            
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(InternalMessage obj)
        {
            ModelState.Clear();
            if(db.DefaultUser.Any(x => x.Email == obj.ReceiverEmail))
            {
                var receiver = db.DefaultUser.FirstOrDefault(x => x.Email == obj.ReceiverEmail);
                obj.ReceiverId = receiver.Id;
                obj.ReceiverEmail = receiver.Email;
            }
            else
            {
                ModelState.AddModelError("ReceiverEmail", "Email not found in directory");
            }

            if (obj.Subject == null || obj.Subject == "")
            {
                ModelState.AddModelError("Subject", "Subject field is required");
            }
            else if (obj.Message == null || obj.Message == "")
            {
                ModelState.AddModelError("Message", "Message field is required");
            }

            var user = db.DefaultUser.Find(HttpContext.Session.GetInt32("UserId"));

            obj.SenderEmail = user.Email;
            obj.SenderId = user.Id;

            if (ModelState.IsValid)
            {
                db.InternalMessaging.Add(obj);
                db.SaveChanges();

                TempData["Success"] = "Message Sent";
                return RedirectToAction("Messenger");
            }
            TempData["Error"] = "Message failed to send";
            return View();
        }

        public IActionResult ReadMessage(int? id)
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var message = db.InternalMessaging.Find(id);
            return View(message);
        }

        public IActionResult ReadSentMessage(int? id)
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }

            if (id == null || id == 0)
            {
                return NotFound();
            }

            var message = db.InternalMessaging.Find(id);
            return View(message);
        }

        public IActionResult DeleteMessage(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var message = db.InternalMessaging.Find(id);
            db.InternalMessaging.Remove(message);
            db.SaveChanges();

            return RedirectToAction("Messenger");
        }
    }
}
