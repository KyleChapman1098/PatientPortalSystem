using Microsoft.AspNetCore.Mvc;
using PatientPortalSystem.Models;
using Microsoft.AspNetCore.Http;
using PatientPortalSystem.Data;
using Microsoft.IdentityModel.Tokens;
using PatientPortalSystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace PatientPortalSystem.Controllers
{
    public class PatientController : Controller
    {

        private readonly ApplicationDbContext db;

        public PatientController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index() //Handles when someone logs in as a patient, checking whether they have a patient record in the patient table. If the record does not exist, user is sent to the patient intake process
        {
            if(HttpContext.Session.GetString("IsVerified") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }

            if (db.Patient.Any(x => x.Id == HttpContext.Session.GetInt32("UserId")))
            {
                return RedirectToAction("PatientInfo");
            }
            else
            {
                return RedirectToAction("SelectDoctor");
            }
        }

        public IActionResult SelectDoctor() //Method which takes users to a list of all doctors at the clinic where they can select the one assigned to them
        {
            IEnumerable<User> doctorList = db.DefaultUser.Where(x => x.Role == "Doctor");

            return View(doctorList);
        }

        public IActionResult PatientIntake(int id)
        {
            if(db.Patient.Any(x => x.Id == HttpContext.Session.GetInt32("UserId"))) //If the patient already exists, we just update their current doctor and skip creating a new patient row
            {
                Patient existingPatient = db.Patient.FirstOrDefault(x => x.Id == HttpContext.Session.GetInt32("UserId"));
                existingPatient.PhysicianId = id;

                db.Patient.Update(existingPatient);
                db.SaveChanges();
                TempData["Success"] = "Doctor Changed Successfully";
                return RedirectToAction("PatientInfo");
            }

            Patient newPatient = new Patient(); //If the patient does not exists then a new patient is created and the user completes the intake process
            newPatient.PhysicianId = id;
           
            return View(newPatient);
        }

        [HttpPost]
        public IActionResult PatientIntake(Patient obj) //Ensures users have filled out all intake information and places information in the database
        {
            obj.Id = (int)HttpContext.Session.GetInt32("UserId");
            if (ModelState.IsValid)
            {
                db.Patient.Add(obj);
                db.SaveChanges();
                return RedirectToAction("PatientInfo");
            }
            return View(obj);
        }
        
        public IActionResult UpdateAccount() //Takes user to a page where they can update their account info (username, email, etc.)
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
        public IActionResult UpdateAccount(User obj) //Ensures updated user information is not duplicate in the database and updates their entry in the defaultuser table
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

        public IActionResult Messenger() //Takes user to their inbox/outbox of internal messages
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

        public IActionResult SendMessage() //Takes user to a page where they can create an internal message using emails on file in the database
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }
            
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(InternalMessage obj) //Checks thats user created message is going to a valid email and is filled out before creating message entry in database
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

        public IActionResult ReadMessage(int? id) //Allows user to open messages in their inbox
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

        public IActionResult ReadSentMessage(int? id) //Allows user to open messages they sent previously
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

        public IActionResult DeleteMessage(int? id) //Deletes message entry from the database
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
            db.InternalMessaging.Remove(message);
            db.SaveChanges();

            return RedirectToAction("Messenger");
        }

        public IActionResult PatientInfo() //Takes user to page which displays all their current account/medical info on file
        {
            if (HttpContext.Session.GetString("IsVerified") != "true") //Check that user is logged in as a patient
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }
            if (!db.Patient.Any(x => x.Id == HttpContext.Session.GetInt32("UserId"))) //Check that user has already completed intake process
            {
                return RedirectToAction("SelectDoctor");
            }

            //Gathering information from multiple tables to put in the ViewModel and pass to the View
            var user = db.DefaultUser.Find(HttpContext.Session.GetInt32("UserId"));
            var patient = db.Patient.FirstOrDefault(x => x.Id == HttpContext.Session.GetInt32("UserId"));
            var insurance = db.Insurance.FirstOrDefault(x => x.Id == HttpContext.Session.GetInt32("UserId"));
            var doctor = db.DefaultUser.Find(patient.PhysicianId);

            PatientViewModel viewModel = new PatientViewModel //Patient ViewModel object is just an object which holds all information from User, Patient, and Insurance so all three tables can be updated and pulled from on the same page
            {
                //User Attributes
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DOB = user.DOB,
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                Address = user.Address,
                Phone = user.Phone,
                DateRegistered = user.DateRegistered,
                SSN = user.SSN,
                Role = user.Role,

                //Patient Attributes
                PatientId = patient.PatientId,
                PhysicianId = patient.PhysicianId,
                PhysicianName = doctor.FirstName + " " + doctor.LastName,
                EmergencyContactFirstName = patient.EmergencyContactFirstName,
                EmergencyContactLastName = patient.EmergencyContactLastName,
                EmergencyContactAddress = patient.EmergencyContactAddress,
                EmergencyContactPhone = patient.EmergencyContactPhone,
                PreviousClinicName = patient.PreviousClinicName,
                PreviousClinicAddress = patient.PreviousClinicAddress,
                PreviousClinicPhone = patient.PreviousClinicPhone,
                Allergies = patient.Allergies,
                Medications = patient.Medications,
                Conditions = patient.Conditions,
                SendReminders = patient.SendReminders,
            };

            if(insurance != null)
            {
                viewModel.InsuranceId = insurance.InsuranceId;
                viewModel.ProviderName = insurance.ProviderName;
                viewModel.PolicyNumber = insurance.PolicyNumber;
            }

            return View(viewModel);
        }

        public IActionResult UpdateContact(int? id) //Allows user to update their emergency contact info
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }

            var patient = db.Patient.Find(id);
            return View(patient);
        }

        [HttpPost]
        public IActionResult UpdateContact(Patient obj) //Checks that newly entered emergency contact info is valid before updating the entry in the database
        {
            if (ModelState.IsValid)
            {
                obj.Id = (int)HttpContext.Session.GetInt32("UserId");

                db.Patient.Update(obj);
                db.SaveChanges();

                TempData["Success"] = "Emergency Contact Updated";
                return RedirectToAction("PatientInfo");
            }
            TempData["Error"] = "Failed to Update";
            return View(obj);
        }

        public IActionResult UpdateMedHistory(int? id) //Allows user to update their medical history info
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }

            var patient = db.Patient.Find(id);
            return View(patient);
        }

        [HttpPost]
        public IActionResult UpdateMedHistory(Patient obj) //Checks that newly entered medical history info is valid before updating the database
        {
            if (ModelState.IsValid)
            {
                obj.Id = (int)HttpContext.Session.GetInt32("UserId");

                db.Patient.Update(obj);
                db.SaveChanges();

                TempData["Success"] = "Medical History Updated";
                return RedirectToAction("PatientInfo");
            }
            TempData["Error"] = "Failed to Update";
            return View(obj);
        }

        public IActionResult AddInsurance() //Allows user to enter their insurance provider and policy number
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddInsurance(Insurance obj) //Checks that insurance info is complete before creating new entry in insurance table in the database
        {
            ModelState.Clear();
            if(obj.ProviderName == null)
            {
                ModelState.AddModelError("ProviderName", "This is a required field");
            }
            if(obj.PolicyNumber == null)
            {
                ModelState.AddModelError("PolicyNumber", "This is a required field");
            }
            if(ModelState.IsValid)
            {
                obj.Id = (int)HttpContext.Session.GetInt32("UserId");

                db.Insurance.Add(obj);
                db.SaveChanges();

                TempData["Success"] = "Insurance Added Successfully";
                return RedirectToAction("PatientInfo");
            }
            TempData["Error"] = "Failed to Add Insurance";
            return View(obj);
        }

        public IActionResult UpdateInsurance(int? InsuranceId) //Allows user to update previously entered insurance information
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }

            var insurance = db.Insurance.Find(InsuranceId);
            return View(insurance);
        }

        [HttpPost]
        public IActionResult UpdateInsurance(Insurance obj) //Checks that updated insurance info is complete before updating the database
        {
            if(ModelState.IsValid)
            {
                db.Insurance.Update(obj);
                db.SaveChanges();


                TempData["Success"] = "Insurance Updated Successfully";
                return RedirectToAction("PatientInfo");
            }
            TempData["Error"] = "Insurance Failed to Update";
            return View(obj);
        }

        public IActionResult ToggleReminders(int? id) //Toggles the value of SendReminders 
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }

            var patient = db.Patient.Find(id);

            if(patient.SendReminders)
            {
                patient.SendReminders = false;
                db.Patient.Update(patient);
                db.SaveChanges();

                TempData["Success"] = "Reminder Turned Off";
                return RedirectToAction("PatientInfo");
            }
            else
            {
                patient.SendReminders = true;
                db.Patient.Update(patient);
                db.SaveChanges();

                TempData["Success"] = "Reminder Turned On";
                return RedirectToAction("PatientInfo");
            }
            
        }

        public IActionResult Appointments()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }

            var patient = db.Patient.FirstOrDefault(x => x.Id == HttpContext.Session.GetInt32("UserId"));
            IEnumerable<Appointment> appointments = db.Appointment.Where(x => x.PatientId == patient.PatientId);
            
            return View(appointments);
        }

        public IActionResult Requests()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }

            var patient = db.Patient.FirstOrDefault(x => x.Id == HttpContext.Session.GetInt32("UserId"));
            var requests = db.Request.Where(x => x.PatientId == patient.PatientId);

            return View(requests);
        }

        public IActionResult CreateRequest()
        {
            if (HttpContext.Session.GetString("IsVerified") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }

            var user = db.DefaultUser.Find(HttpContext.Session.GetInt32("UserId"));
            var patient = db.Patient.FirstOrDefault(x=> x.Id == HttpContext.Session.GetInt32("UserId"));
            var doctor = db.DefaultUser.Find(patient.PhysicianId);

            Request request = new Request
            {
                PatientId = patient.PatientId,
                DoctorId = doctor.Id,
                PatientUsername = user.Username,
                PatientName = user.FirstName + " " + user.LastName,
                DoctorName = doctor.FirstName + " " + doctor.LastName,
                PatientPhone = user.Phone,
            };

            return View(request);
        }

        [HttpPost]
        public IActionResult CreateRequest(Request obj)
        {
            if (ModelState.IsValid)
            {
                db.Request.Add(obj);
                db.SaveChanges();

                TempData["Success"] = "Request Sent";
                return RedirectToAction("Requests");
            }
            
            return View(obj);
            
        }

        
    }
}
