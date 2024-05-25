namespace API.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime SendDate { get; set; }
        public string EmailBody { get; set; } = string.Empty;
        public string SMSMessage { get; set; } = string.Empty;
        public string WhatsMessage { get; set; } = string.Empty;

    }
}
