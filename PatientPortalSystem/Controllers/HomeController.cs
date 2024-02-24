using Microsoft.AspNetCore.Mvc;
using PatientPortalSystem.Data;
using PatientPortalSystem.Models;
using System.Diagnostics;
using System.Drawing.Text;

namespace PatientPortalSystem.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
		{
			_logger = logger;
			db = context;
		}

	
		public IActionResult Index()
		{
			User admin = new User();
			admin.FirstName = "system";
			admin.LastName = "Admin";
			admin.DOB = new DateOnly(2024, 1, 1);
			admin.Email = "admin@admin.net";
			admin.Address = "address";
			admin.Phone = "3047107102";
			admin.Username = "admin";
			admin.Password = "password";
			admin.Role = "Admin";
			admin.SSN = "340232342";
			if(!db.DefaultUser.Any(x => x.Role == "admin"))
			{
				db.Add(admin);
				db.SaveChanges();
			}
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
