using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace PatientPortalSystem.Models
{
    public class Bill
    {
        [Key]
        public int BillId { get; set; }

        public string BillReason { get; set; }

        public float BillAmount { get; set; }

        public DateOnly BillDate { get; set; }

        public DateOnly BillDueDate { get; set; }
    }
}
