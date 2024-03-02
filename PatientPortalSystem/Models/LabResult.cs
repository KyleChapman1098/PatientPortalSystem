using System.ComponentModel.DataAnnotations;

namespace PatientPortalSystem.Models
{
    public class LabResult
    {
        [Key]
        public int ResultId { get; set; }

        public int LabId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public string ResultInfo { get; set; }

    }
}
