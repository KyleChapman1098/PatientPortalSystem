using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PatientPortalSystem.Models
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        [DisplayName("Name")]
        public string DoctorName { get; set; }

        [DisplayName("Username")]
        public string PatientUsername { get; set; }

        [DisplayName("Name")]
        public string PatientName { get; set; }

        [DisplayName("Phone")]
        public string PatientPhone { get; set; }

        [DisplayName("Request Type")]
        public string RequestType { get; set; }

        public string Message { get; set; }

        [DisplayName("Time Received")]
        public DateTime MessageTime { get; set; } = DateTime.Now;
    }
}
