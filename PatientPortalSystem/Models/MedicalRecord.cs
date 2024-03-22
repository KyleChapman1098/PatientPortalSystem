using System.ComponentModel.DataAnnotations;

namespace PatientPortalSystem.Models
{
    public class MedicalRecord
    {
        [Key]
        public int RecordId { get; set; }

        public int PatientId { get; set; }

        public int AppointmentId { get; set; }

        public string DoctorDiagnosis { get; set; }

        public string DoctorComments { get; set; }

    }
}
