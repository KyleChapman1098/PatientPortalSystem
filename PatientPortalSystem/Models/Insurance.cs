using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace PatientPortalSystem.Models
{
    public class Insurance
    {
        [Key]
        public int InsuranceId { get; set; }

        public int Id { get; set; }

        [DisplayName("Provider Name")]
        public string ProviderName { get; set; }

        [DisplayName("Policy Number")]
        public string PolicyNumber { get; set; }
    }
}
