using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PatientPortalSystem.Models
{
    public class InternalMessage
    {
        [Key]
        public int MessageId { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DisplayName("To")]
        public string ReceiverEmail { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DisplayName("From")]
        public string SenderEmail { get; set; } = "TBD";

        public string Subject { get; set; }

        public string Message { get; set; }

        [DisplayName("Message Time")]
        public DateTime MessageTime { get; set; } = DateTime.Now;
    }
}
