using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientPortalSystem.Models
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }

        public int Id { get; set; } 

        

    }
}
