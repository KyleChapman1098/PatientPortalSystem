using System.ComponentModel.DataAnnotations;

namespace PatientPortalSystem.Models
{
    public class Lab
    {
        [Key]
        public int LabId { get; set; }

        public string NameLab { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string TestType { get; set; }

    }
}
