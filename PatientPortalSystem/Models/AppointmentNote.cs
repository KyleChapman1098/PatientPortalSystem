using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PatientPortalSystem.Models
{
    public class AppointmentNote
    {
        [Key]
        public int AppointmentNoteId { get; set; }

        public int AppointmentId { get; set; }

        public DateOnly DateInfo { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public int Age { get; set; }

        public float Weight { get; set; }

        public float Height { get; set; }

        public int HeartRate { get; set; }

        public float BloodPressure { get; set; }

        public float Oxygen { get; set; }

        public string NurseComments { get; set; }
    }
}
