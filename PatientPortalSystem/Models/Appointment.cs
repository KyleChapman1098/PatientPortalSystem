using System.ComponentModel.DataAnnotations;

namespace PatientPortalSystem.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public int SchedulerId { get; set; }

        public string PatientName { get; set; }

        public string DoctorName { get; set; }

        public string SchedulerName { get; set; }

        public DateTime AppointmentDate { get; set; }  

        public string Reason { get; set; }
    }
}
