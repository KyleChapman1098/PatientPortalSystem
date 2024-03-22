using Microsoft.AspNetCore.Mvc;
using PatientPortalSystem.Data;
using PatientPortalSystem.Models;
using PatientPortalSystem.Models.ViewModels;

namespace PatientPortalSystem.Controllers
{
    public class ReceptionistController : Controller
    {
        private readonly ApplicationDbContext db;

        public ReceptionistController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Calendar(DateOnly? date, int? doctorID)
        {
            if (HttpContext.Session.GetString("IsReceptionist") != "true")
            {
                TempData["Error"] = "You must be logged in as a receptionist to view this page";
                return RedirectToAction("Login", "Account");
            }
            if(doctorID == null)
            {
                return RedirectToAction("SelectDoctor");
            }

            var doctor = db.DefaultUser.Find(doctorID);
            if (date == null)
            {
                date = DateOnly.FromDateTime(DateTime.Now);
                CalendarViewModel calendarToday = new CalendarViewModel
                {
                    Date = (DateOnly)date,
                    DoctorID = (int)doctorID,
                    DoctorName = doctor.FirstName + " " + doctor.LastName,
                    Appointments = db.Appointment.Where(x => DateOnly.FromDateTime(x.AppointmentDate) == date && x.DoctorId == doctor.Id),
                };
                return View(calendarToday);
            }
            CalendarViewModel calendarDay = new CalendarViewModel
            {
                Date = (DateOnly)date,
                DoctorID = (int)doctorID,
                DoctorName = doctor.FirstName + " " + doctor.LastName,
                Appointments = db.Appointment.Where(x => DateOnly.FromDateTime(x.AppointmentDate) == date && x.DoctorId == doctor.Id),
            };

            return View(calendarDay);
        }

        [HttpPost]
        public IActionResult Calendar(CalendarViewModel obj)
        {
            return Calendar(obj.Date, obj.DoctorID);
        }

        public IActionResult SelectDoctor()
        {
            if (HttpContext.Session.GetString("IsReceptionist") != "true")
            {
                TempData["Error"] = "You must be logged in as a receptionist to view this page";
                return RedirectToAction("Login", "Account");
            }
            IEnumerable <User> doctorList = db.DefaultUser.Where(x => x.Role == "Doctor");
            return View(doctorList);
        }

        public IActionResult Requests()
        {
            if (HttpContext.Session.GetString("IsReceptionist") != "true")
            {
                TempData["Error"] = "You must be logged in as a receptionist to view this page";
                return RedirectToAction("Login", "Account");
            }

            IEnumerable<Request> requests = db.Request.AsEnumerable();
            return View(requests);            
        }

        public IActionResult ViewRequest(int? id)
        {
            if (HttpContext.Session.GetString("IsReceptionist") != "true")
            {
                TempData["Error"] = "You must be logged in as a receptionist to view this page";
                return RedirectToAction("Login", "Account");
            }

            var request = db.Request.Find(id);
            return View(request);
        }

        public IActionResult CloseRequest(int? id)
        {
            if (HttpContext.Session.GetString("IsReceptionist") != "true")
            {
                TempData["Error"] = "You must be logged in as a receptionist to view this page";
                return RedirectToAction("Login", "Account");
            }

            if(id == null)
            {
                return NotFound();
            }

            var request = db.Request.Find(id);

            db.Request.Remove(request);
            db.SaveChanges();

            TempData["Success"] = "Request Closed";
            return RedirectToAction("Requests");
        }

        public IActionResult CreateAppointment(int? userId)
        {
            if (HttpContext.Session.GetString("IsReceptionist") != "true")
            {
                TempData["Error"] = "You must be logged in as a receptionist to view this page";
                return RedirectToAction("Login", "Account");
            }
            
            if(userId == null)
            {
                return RedirectToAction("SelectPatient");
            }

            var patient = db.Patient.FirstOrDefault(x => x.Id == userId);
            var user = db.DefaultUser.Find(userId);
            var doctor = db.DefaultUser.Find(patient.PhysicianId);
            var scheduler = db.DefaultUser.Find(HttpContext.Session.GetInt32("UserId"));
            AppointmentViewModel appointment = new AppointmentViewModel
            {
                //Appointment Attributes
                PatientId = patient.PatientId,
                DoctorId = doctor.Id,
                SchedulerId = scheduler.Id,
                AppointmentDate = DateOnly.FromDateTime(DateTime.Now),

                //User Attributes
                UserId = user.Id,
                Username = user.Username,
                UserEmail = user.Email,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,

                //Doctor Attributes
                DoctorFirstName = doctor.FirstName,
                DoctorLastName = doctor.LastName,

                //Scheduler Attributes
                SchedulerFirstName = scheduler.FirstName,
                SchedulerLastName = scheduler.LastName,
            };
            return View(appointment);
        }

        [HttpPost]
        public IActionResult CreateAppointment(AppointmentViewModel appointment)
        {
            if(appointment.AppointmentDate.CompareTo(DateOnly.FromDateTime(DateTime.Now)) < 0)
            {
                ModelState.AddModelError("AppointmentDate", "This date has already passed");
            }

            Appointment newAppointment = new Appointment
            {
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                SchedulerId = appointment.SchedulerId,
                PatientName = appointment.UserFirstName + " " + appointment.UserLastName,
                DoctorName = appointment.DoctorFirstName + " " + appointment.DoctorLastName,
                SchedulerName = appointment.SchedulerFirstName + " " + appointment.SchedulerLastName,
                AppointmentDate = appointment.AppointmentDate.ToDateTime(appointment.AppointmentTime),
                Reason = appointment.Reason,
            };
            if(db.Appointment.Any(x => x.AppointmentDate == newAppointment.AppointmentDate && x.DoctorId == newAppointment.DoctorId))
            {
                ModelState.AddModelError("AppointmentTime", "An appointment with this date and time already exists");
            }

            if (ModelState.IsValid)
            {
                
                db.Appointment.Add(newAppointment);
                db.SaveChanges();

                TempData["Success"] = "Appointment Created";
                return RedirectToAction("Calendar", new { date = appointment.AppointmentDate, doctorid = appointment.DoctorId });
            }
            return View(appointment);
        }

        public IActionResult SelectPatient()
        {
            if (HttpContext.Session.GetString("IsReceptionist") != "true")
            {
                TempData["Error"] = "You must be logged in as a receptionist to view this page";
                return RedirectToAction("Login", "Account");
            }
            
            IEnumerable<User> patients = db.DefaultUser.Where(x => x.Role == "Patient");
            return View(patients);
        }

        public IActionResult UpdateAppointment(int? id)
        {
            if (HttpContext.Session.GetString("IsReceptionist") != "true")
            {
                TempData["Error"] = "You must be logged in as a receptionist to view this page";
                return RedirectToAction("Login", "Account");
            }

            var appointment = db.Appointment.Find(id);
            var patient = db.Patient.Find(appointment.PatientId);
            var user = db.DefaultUser.Find(patient.Id);
            var doctor = db.DefaultUser.Find(appointment.DoctorId);
            var scheduler = db.DefaultUser.Find(appointment.SchedulerId);

            AppointmentViewModel appointmentModel = new AppointmentViewModel
            {
                //Appointment Attributes
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                SchedulerId = appointment.SchedulerId,
                AppointmentDate = DateOnly.FromDateTime(appointment.AppointmentDate),
                AppointmentTime = TimeOnly.FromDateTime(appointment.AppointmentDate),
                Reason = appointment.Reason,

                //User Attributes
                UserId = user.Id,
                Username = user.Username,
                UserEmail = user.Email,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,

                //Doctor Attributes
                DoctorFirstName = doctor.FirstName,
                DoctorLastName = doctor.LastName,

                //Scheduler Attributes
                SchedulerFirstName = scheduler.FirstName,
                SchedulerLastName = scheduler.LastName,
            };

            return View(appointmentModel);
        }

        [HttpPost]
        public IActionResult UpdateAppointment(AppointmentViewModel appointment)
        {
            if (appointment.AppointmentDate.CompareTo(DateOnly.FromDateTime(DateTime.Now)) < 0)
            {
                ModelState.AddModelError("AppointmentDate", "This date has already passed");
            }

            Appointment newAppointment = new Appointment
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                SchedulerId = appointment.SchedulerId,
                PatientName = appointment.UserFirstName + " " + appointment.UserLastName,
                DoctorName = appointment.DoctorFirstName + " " + appointment.DoctorLastName,
                SchedulerName = appointment.SchedulerFirstName + " " + appointment.SchedulerLastName,
                AppointmentDate = appointment.AppointmentDate.ToDateTime(appointment.AppointmentTime),
                Reason = appointment.Reason,
            };
            if (db.Appointment.Any(x => x.AppointmentDate == newAppointment.AppointmentDate && x.DoctorId == newAppointment.DoctorId))
            {
                ModelState.AddModelError("AppointmentTime", "An appointment with this date and time already exists");
            }

            if (ModelState.IsValid)
            {

                db.Appointment.Update(newAppointment);
                db.SaveChanges();

                TempData["Success"] = "Appointment Updated";
                return RedirectToAction("Calendar", new { date = appointment.AppointmentDate, doctorid = appointment.DoctorId });
            }
            TempData["Error"] = "Appointment Update Failed";
            return View(appointment);
        }

        public IActionResult DeleteAppointment(int? id)
        {
            if (HttpContext.Session.GetString("IsReceptionist") != "true")
            {
                TempData["Error"] = "You must be logged in as a receptionist to view this page";
                return RedirectToAction("Login", "Account");
            }
            var appointment = db.Appointment.Find(id);

            db.Appointment.Remove(appointment);
            db.SaveChanges();

            TempData["Success"] = "Appointment Cancelled";
            return RedirectToAction("Calendar", new { date = DateOnly.FromDateTime(appointment.AppointmentDate), doctorid = appointment.DoctorId });
        }

        
    }
}
