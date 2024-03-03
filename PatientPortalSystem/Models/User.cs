using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PatientPortalSystem.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }

        [DisplayName("First Name")]
		public string FirstName {  get; set; }

		[DisplayName("Last Name")]
		public string LastName { get; set; }

		[DisplayName("Date of Birth")]
		public DateOnly DOB { get; set; }

		public string Username {  get; set; }

		[DataType(DataType.Password)]
		
		public string Password { get; set; }

		public string Address {  get; set; }

		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }

		[DisplayName("Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public string Phone { get; set; }

		[DisplayName("Date Registered")]
		public DateOnly DateRegistered { get; set; } = DateOnly.FromDateTime(DateTime.Now);

		[DisplayName("Social Security Number")]
		public string SSN { get; set; }

		public string Role { get; set; } = "Patient";

	}
}
