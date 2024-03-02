using System.ComponentModel.DataAnnotations;

namespace PatientPortalSystem.Models
{
    public class Pharmacy
    {
        [Key]
        public int PharmacyId { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }


    }
}
