using Microsoft.AspNetCore.Mvc;
using PatientPortalSystem.Data;
using PatientPortalSystem.Models;

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
            return View();
        }

        public IActionResult AccessPatientFile(int patientId)
        {
            if (HttpContext.Session.GetString("IsNurse") != "true")
            {
                TempData["Error"] = "You must be logged in as a nurse to access patient files";
                return RedirectToAction("Login", "Account");
            }

            // Retrieve patient details from the database
            var patient = _db.Patient.FirstOrDefault(p => p.Id== patientId);
            if (patient == null)
            {
                TempData["Error"] = "Patient not found";
                return RedirectToAction("Index");
            }

            // Perform actions related to accessing patient files

            return View(patient);
        }

        public IActionResult GenerateAppointmentNotes(int appointmentId)
        {
            if (HttpContext.Session.GetString("IsNurse") != "true")
            {
                TempData["Error"] = "You must be logged in as a nurse to generate appointment notes";
                return RedirectToAction("Login", "Account");
            }

            // Retrieve appointment details from the database
            var appointment = _db.Appointment.FirstOrDefault(a => a.AppointmentId == appointmentId);
            if (appointment == null)
            {
                TempData["Error"] = "Appointment not found";
                return RedirectToAction("Index");
            }

            // Perform actions related to generating appointment notes

            return View(appointment);
        }
    }
}
