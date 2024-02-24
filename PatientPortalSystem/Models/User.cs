using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace PatientPortalSystem.Models
{
	public class User
	{
		public int Id { get; set; }
        [DisplayName("First Name")]
		public string FirstName {  get; set; }
		[DisplayName("Last Name")]
		public string LastName { get; set; }
		[DisplayName("Date of Birth")]
		public DateOnly DOB { get; set; }
		public string Username {  get; set; }
		public string Password { get; set; }
		public string Address {  get; set; }
		public string Email { get; set; }
		[DisplayName("Phone Number")]
		public string Phone { get; set; }
		public DateTime DateRegistered { get; set; } = DateTime.Now;
		[DisplayName("Social Security Number")]
		public string SSN { get; set; }
		[DefaultValue("Patient")]
		public string Role { get; set; } = "Patient";
	}
}
