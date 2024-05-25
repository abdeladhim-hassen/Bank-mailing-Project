namespace API.Services.CommunicationService
{
    public interface ICommunicationService
    {
        Task SendEmailAsync(string recipient, string subject, string body);
        Task<string> SendSMSAsync(string recipient, string message);
        Task SendWhatsAppMessageAsync(string recipient, string message);
    }
}
