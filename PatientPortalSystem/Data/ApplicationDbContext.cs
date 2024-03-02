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
		
		public DbSet<Appointment> Appointment { get; set; }

		public DbSet<Bill> Bill { get; set; }

		public DbSet<Insurance> Insurance { get; set; }

		public DbSet<InternalMessaging> InternalMessaging { get; set; }

		public DbSet<Lab> Lab { get; set; }

		public DbSet<LabResult> LabResult { get; set; }

		public DbSet<Patient> Patient { get; set; }

		public DbSet<Pharmacy> Pharmacy { get; set;}

		public DbSet<Prescription> Prescription { get; set; }

		public DbSet<Staff> Staff { get; set; }
		

	}
}
