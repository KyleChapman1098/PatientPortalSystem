using System.ComponentModel.DataAnnotations;

namespace PatientPortalSystem.Models.ViewModels
{
    public partial class CalendarViewModel
    {
        [Key]
        public DateOnly Date {  get; set; }

        public int DoctorID { get; set; }

        public string DoctorName { get; set; }

        public IEnumerable<Appointment> Appointments { get; set; }
    }
}
