using Microsoft.AspNetCore.Mvc;

namespace PatientPortalSystem.Controllers
{
    public class NurseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
