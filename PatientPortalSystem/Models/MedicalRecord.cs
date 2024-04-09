using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PatientPortalSystem.Models
{
    public class MedicalRecord
    {
        [Key]
        public int RecordId { get; set; }

        public int PatientId { get; set; }

        public int AppointmentId { get; set; }

        [DisplayName("Record Date")]
        public DateOnly RecordDate {  get; set; }

        [DisplayName("Doctor Diagnosis")]
        public string DoctorDiagnosis { get; set; }

        [DisplayName("Doctor Comments")]
        public string DoctorComments { get; set; }
    }
}
