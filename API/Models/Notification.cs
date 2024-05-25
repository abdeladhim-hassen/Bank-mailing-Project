using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string EmailBody { get; set; } = null!;

        [Required]
        public string SMSMessage { get; set; } = null!;

        [Required]
        public string WhatsMessage { get; set; } = null!;


        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Required]
        public DateTime SendDate { get; set; }

        public bool IsDelivered { get; set; } = false;

        [InverseProperty("Notification")]
        public virtual ICollection<ClientNotification> ClientNotifications { get; set; } = [];
    }
}
