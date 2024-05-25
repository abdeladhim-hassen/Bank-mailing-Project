using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class ClientNotification
    {
        [Key, Column(Order = 0)]
        public int ClientId { get; set; }

        [Key, Column(Order = 1)]
        public int NotificationId { get; set; }

        [ForeignKey("ClientId")]
        [InverseProperty("ClientNotifications")]
        public virtual Client Client { get; set; } = null!;

        [ForeignKey("NotificationId")]
        [InverseProperty("ClientNotifications")]
        public virtual Notification Notification { get; set; } = null!;

    }
}
