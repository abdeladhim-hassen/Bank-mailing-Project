using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class UpdateNotificationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime SendDate { get; set; }
        public string EmailBody { get; set; } = string.Empty;
        public string SMSMessage { get; set; } = string.Empty;
        public string WhatsMessage { get; set; } = string.Empty;
    }
}
