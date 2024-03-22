using Microsoft.Build.ObjectModelRemoting;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PatientPortalSystem.Models.ViewModels
{
    public partial class AppointmentViewModel
    {
        //Appointment Attributes
        [Key]
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }  

        public int DoctorId { get; set; }

        public int SchedulerId { get; set; }

        [DisplayName("Appointment Date")]
        public DateOnly AppointmentDate { get; set; }

        [DisplayName("Appointment Time")]
        public TimeOnly AppointmentTime { get; set; }

        [DisplayName("Reason for Appointment")]
        public string Reason { get; set; }

        //User Attributes
        public int UserId { get; set; }

        public string Username { get; set; }

        public string UserEmail { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        //Doctor Attributes
        public string DoctorFirstName { get; set; }

        public string DoctorLastName { get; set;}

        //Scheduler Attributes
        public string SchedulerFirstName { get; set; }

        public string SchedulerLastName { get; set; }



    }
}
