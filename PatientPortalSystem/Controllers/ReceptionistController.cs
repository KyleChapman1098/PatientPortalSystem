using Microsoft.AspNetCore.Mvc;

namespace PatientPortalSystem.Controllers
{
    public class ReceptionistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
