using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace PatientPortalSystem.Models
{
    public class Insurance
    {
        [Key]
        public int InsuranceId { get; set; }

        public int Id { get; set; }

        public string ProviderName { get; set; }

        public string PolicyNumber { get; set; }
    }
}
