using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientPortalSystem.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        public int PhysicianId { get; set; }

        public int Id { get; set; }

        [DisplayName("First Name")]
        public string EmergencyContactFirstName { get; set; }

        [DisplayName("Last Name")]
        public string EmergencyContactLastName { get; set; }

        [DisplayName("Phone Number")]
        public string EmergencyContactPhone { get; set; }

        [DisplayName("Address")]
        public string EmergencyContactAddress { get; set; }

        [DisplayName("Clinic Name")]
        public string PreviousClinicName { get; set; } 

        [DisplayName("Clinic Phone Number")]
        public string PreviousClinicPhone { get; set; } 

        [DisplayName("Clinic Address")]
        public string PreviousClinicAddress { get; set; } 

        public string Allergies { get; set; } 

        public string Medications { get; set; } 

        [DisplayName("Pre-Existing Conditions")]
        public string Conditions { get; set; } 

        [DisplayName("Send Reminders")]
        public Boolean SendReminders { get; set; }







    }
}
