using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PatientPortalSystem.Models.ViewModels
{
    public partial class MedicalStaffAppointmentViewModel
    {
        //Appointment Attributes
        [Key]
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public int SchedulerId { get; set; }

        [DisplayName("Patient Name")]
        public string PatientName { get; set; }

        [DisplayName("Doctor Name")]
        public string DoctorName { get; set; }

        [DisplayName("Scheduler Name")]
        public string SchedulerName { get; set; }

        [DisplayName("Appointment Date")]
        public DateTime AppointmentDate { get; set; }

        public string Reason { get; set; }

        //Appointment Note Attributes
        public int AppointmentNoteId { get; set; }

        public DateOnly DateInfo { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public int Age { get; set; }

        public float Weight { get; set; }

        public float Height { get; set; }

        public int HeartRate { get; set; }

        public float BloodPressure { get; set; }

        public float Oxygen { get; set; }

        public string NurseComments { get; set; }

        //Medical Record Attributes
        public int RecordId { get; set; }

        public string DoctorDiagnosis { get; set; }

        public string DoctorComments { get; set; }
    }
}
