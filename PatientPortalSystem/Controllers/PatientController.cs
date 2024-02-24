using Microsoft.AspNetCore.Mvc;

namespace PatientPortalSystem.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
