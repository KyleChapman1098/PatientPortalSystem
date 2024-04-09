using System.ComponentModel.DataAnnotations;

namespace PatientPortalSystem.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public int PharmacyId { get; set; }

        public string MedicineInfo { get; set; }
    }
}
