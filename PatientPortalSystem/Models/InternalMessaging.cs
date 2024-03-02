using System.ComponentModel.DataAnnotations;

namespace PatientPortalSystem.Models
{
    public class InternalMessaging
    {
        [Key]
        public int MessageId { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public string Message { get; set; }
    }
}
