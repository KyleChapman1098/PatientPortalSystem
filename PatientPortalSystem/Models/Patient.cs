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

        public string EmergencyContact { get; set; }

        public int InsuranceId { get; set; }

        public string MedicalHistory { get; set; }

        public Boolean SendReminders { get; set; }







    }
}
