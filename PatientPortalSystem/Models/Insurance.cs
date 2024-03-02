using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace PatientPortalSystem.Models
{
    public class Insurance
    {
        [Key]
        public int InsuranceId { get; set; }

        public string ProviderName { get; set; }

        public int PolicyNumber { get; set; }

        public string CoverageDetails { get; set; }

        public float CopayAmount { get; set; }
    }
}
