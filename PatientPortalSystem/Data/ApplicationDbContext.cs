using Microsoft.EntityFrameworkCore;
using PatientPortalSystem.Models;
using PatientPortalSystem.Models.ViewModels;

namespace PatientPortalSystem.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<User> DefaultUser { get; set; }
		
		public DbSet<Appointment> Appointment { get; set; }

		public DbSet<AppointmentNote> AppointmentNote { get; set; }

		public DbSet<Request> Request { get; set; }

		public DbSet<Bill> Bill { get; set; }

		public DbSet<HIPAA> HIPAA { get; set; }

		public DbSet<Insurance> Insurance { get; set; }

		public DbSet<InternalMessage> InternalMessaging { get; set; }

		public DbSet<Lab> Lab { get; set; }

		public DbSet<LabResult> LabResult { get; set; }

		public DbSet<MedicalRecord> MedicalRecord { get; set; }

		public DbSet<Patient> Patient { get; set; }

		public DbSet<Pharmacy> Pharmacy { get; set;}

		public DbSet<Prescription> Prescription { get; set; }

		public DbSet<Staff> Staff { get; set; }
	    public DbSet<PatientPortalSystem.Models.ViewModels.AppointmentViewModel> AppointmentViewModel { get; set; } = default!;
	}
}
