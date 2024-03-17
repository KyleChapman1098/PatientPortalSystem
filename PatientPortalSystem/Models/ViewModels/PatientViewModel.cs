using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PatientPortalSystem.Models.ViewModels
{
    public partial class PatientViewModel
    {
        //DefaultUser
        [Key]
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Date of Birth")]
        public DateOnly DOB { get; set; }

        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Address { get; set; }

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

        //Patient
        public int PatientId { get; set; }

        public int PhysicianId { get; set; }

        [DisplayName("Doctor Name")]
        public string PhysicianName { get; set; }

        [DisplayName("First Name")]
        public string EmergencyContactFirstName { get; set; }

        [DisplayName("Last Name")]
        public string EmergencyContactLastName { get; set; }

        [DisplayName("Phone")]
        public string EmergencyContactPhone { get; set; }

        [DisplayName("Address")]
        public string EmergencyContactAddress { get; set; }

        [DisplayName("Clinic Name")]
        public string? PreviousClinicName { get; set; }

        [DisplayName("Clinic Phone Number")]
        public string? PreviousClinicPhone { get; set; }

        [DisplayName("Clinic Address")]
        public string? PreviousClinicAddress { get; set; }

        public string? Allergies { get; set; }

        public string? Medications { get; set; }

        [DisplayName("Pre-Existing Conditions")]
        public string Conditions { get; set; }

        [DisplayName("Send Reminders")]
        public Boolean SendReminders { get; set; }

        //Insurance
        public int InsuranceId { get; set; }

        [DisplayName("Provider Name")]
        public string ProviderName { get; set; }

        [DisplayName("Policy Number")]
        public string PolicyNumber { get; set; }
    }

}
