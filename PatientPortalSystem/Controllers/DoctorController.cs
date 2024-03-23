using Microsoft.AspNetCore.Mvc;
using PatientPortalSystem.Data;
using PatientPortalSystem.Models;
using PatientPortalSystem.Models.ViewModels;

namespace PatientPortalSystem.Controllers
{
    public class DoctorController : Controller
    {

		private readonly ApplicationDbContext db;

		public DoctorController(ApplicationDbContext context)
		{
			db = context;
		}

		public IActionResult Index()
        {
			if (HttpContext.Session.GetString("IsDoctor") != "true")
			{
				TempData["Error"] = "You must be logged in to view this page";
				return RedirectToAction("Login", "Account");
			}
			return View();
        }

		public IActionResult Dashboard()
		{
            if (HttpContext.Session.GetString("IsDoctor") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }
			IEnumerable<Appointment> appointments = db.Appointment.Where(x => DateOnly.FromDateTime(x.AppointmentDate) == DateOnly.FromDateTime(DateTime.Now) && x.DoctorId == HttpContext.Session.GetInt32("UserId"));
			var orderedAppointments = appointments.OrderBy(x => x.AppointmentDate);
            return View(orderedAppointments);
        }

		public IActionResult ViewAppointment(int? id)
		{
            if (HttpContext.Session.GetString("IsDoctor") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }
			var appointment = db.Appointment.Find(id);
			var appointmentNote = db.AppointmentNote.FirstOrDefault(x => x.AppointmentId == id);
			var medicalRecord = db.MedicalRecord.FirstOrDefault(x => x.AppointmentId == id);

			MedicalStaffAppointmentViewModel doctorView = new MedicalStaffAppointmentViewModel
			{
				//Appointment
				AppointmentId = appointment.AppointmentId,
				PatientId = appointment.PatientId,
				DoctorId = appointment.DoctorId,
				SchedulerId = appointment.SchedulerId,
				PatientName = appointment.PatientName,
				SchedulerName = appointment.SchedulerName,
				DoctorName = appointment.DoctorName,
				AppointmentDate = appointment.AppointmentDate,
				Reason = appointment.Reason,
			};

			if(appointmentNote != null)
			{
				doctorView.AppointmentNoteId = appointmentNote.AppointmentNoteId;
				doctorView.DateInfo = appointmentNote.DateInfo;
				doctorView.Age = appointmentNote.Age;
				doctorView.Weight = appointmentNote.Weight;
				doctorView.Height = appointmentNote.Height;
				doctorView.BloodPressure = appointmentNote.BloodPressure;
				doctorView.Oxygen = appointmentNote.Oxygen;
				doctorView.HeartRate = appointmentNote.HeartRate;
				doctorView.NurseComments = appointmentNote.NurseComments;
			}

			if(medicalRecord != null)
			{
				doctorView.RecordId = medicalRecord.RecordId;
				doctorView.DoctorDiagnosis = medicalRecord.DoctorDiagnosis;
				doctorView.DoctorComments = medicalRecord.DoctorComments;
			}

			return View(doctorView);
        }

		public IActionResult CreateMedicalRecord(int? appointmentId)
		{
            if (HttpContext.Session.GetString("IsDoctor") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }
			var appointment = db.Appointment.Find(appointmentId);

			MedicalRecord medicalRecord = new MedicalRecord
			{
				PatientId = appointment.PatientId,
				AppointmentId = appointment.AppointmentId,
			};

			return View(medicalRecord);
        }

		[HttpPost]
		public IActionResult CreateMedicalRecord(MedicalRecord obj)
		{
			if (ModelState.IsValid)
			{
				db.MedicalRecord.Add(obj);
				db.SaveChanges();

				TempData["Success"] = "Medical Record Added";
				return RedirectToAction("ViewAppointment", new { id = obj.AppointmentId });
			}
			return View(obj);
		}

		public IActionResult EditMedicalRecord(int? id)
		{
            if (HttpContext.Session.GetString("IsDoctor") != "true")
            {
                TempData["Error"] = "You must be logged in to view this page";
                return RedirectToAction("Login", "Account");
            }
			var record = db.MedicalRecord.Find(id);
			return View(record);
        }

		[HttpPost]
		public IActionResult EditMedicalRecord(MedicalRecord record)
		{
			if (ModelState.IsValid)
			{
				db.MedicalRecord.Update(record);
				db.SaveChanges();

				TempData["Success"] = "Medical Record Updated";
				return RedirectToAction("ViewAppointment", new {id =  record.AppointmentId});
			}
			return View(record);
		}

		public IActionResult Messenger()
		{
			if (HttpContext.Session.GetString("IsDoctor") != "true")
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
			if (HttpContext.Session.GetString("IsDoctor") != "true")
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
			if (db.DefaultUser.Any(x => x.Email == obj.ReceiverEmail))
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
			if (HttpContext.Session.GetString("IsDoctor") != "true")
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
            if (HttpContext.Session.GetString("IsDoctor") != "true")
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
			if (id == null || id == 0)
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
