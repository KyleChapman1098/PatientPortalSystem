using Microsoft.AspNetCore.Mvc;
using PatientPortalSystem.Data;
using PatientPortalSystem.Models;
using PatientPortalSystem.Models.ViewModels;

namespace PatientPortalSystem.Controllers
{
    public class NurseController : Controller
    {
        private readonly ApplicationDbContext _db;

        public NurseController(ApplicationDbContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("IsNurse") != "true")
            {
                TempData["Error"] = "You must be logged in as a nurse to view this page";
                return RedirectToAction("Login", "Account");
            }
            IEnumerable<Appointment> appointments = _db.Appointment.Where(x => DateOnly.FromDateTime(x.AppointmentDate) == DateOnly.FromDateTime(DateTime.Now));
            return View(appointments);
        }

        public IActionResult ViewAppointment(int id)
        {
            if (HttpContext.Session.GetString("IsNurse") != "true")
            {
                TempData["Error"] = "You must be logged in as a nurse to access patient files";
                return RedirectToAction("Login", "Account");
            }

            var appointment = _db.Appointment.Find(id);
            var appointmentNote = _db.AppointmentNote.FirstOrDefault(x => x.AppointmentId == id);
            var medicalRecord = _db.MedicalRecord.FirstOrDefault(x => x.AppointmentId == id);

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

            if (appointmentNote != null)
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

            if (medicalRecord != null)
            {
                doctorView.RecordId = medicalRecord.RecordId;
                doctorView.DoctorDiagnosis = medicalRecord.DoctorDiagnosis;
                doctorView.DoctorComments = medicalRecord.DoctorComments;
            }

            return View(doctorView);
        }

        public IActionResult CreateAppointmentNotes(int id)
        {
            if (HttpContext.Session.GetString("IsNurse") != "true")
            {
                TempData["Error"] = "You must be logged in as a nurse to generate appointment notes";
                return RedirectToAction("Login", "Account");
            }

            var appointment = _db.Appointment.Find(id);

            AppointmentNote note = new AppointmentNote
            {
                AppointmentId = appointment.AppointmentId,
                
            };

            return View(note);
        }

        [HttpPost]
        public IActionResult CreateAppointmentNotes(AppointmentNote obj)
        {
            if(ModelState.IsValid)
            {
                _db.AppointmentNote.Add(obj);
                _db.SaveChanges();

                TempData["Success"] = "Appointment Note Added";
                return RedirectToAction("ViewAppointment", new {id = obj.AppointmentId});
            }
            return View(obj);
        }
    }
}
