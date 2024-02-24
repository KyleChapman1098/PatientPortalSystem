using Microsoft.EntityFrameworkCore;
using PatientPortalSystem.Models;

namespace PatientPortalSystem.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<User> DefaultUser { get; set; }	

	}
}
