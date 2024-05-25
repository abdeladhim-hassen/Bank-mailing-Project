using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using RestSharp;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace API.Services.CommunicationService
{
    public class CommunicationService(IConfiguration configuration) : ICommunicationService
    {
        private readonly IConfiguration _configuration = configuration;


        public async Task SendEmailAsync(string recipient, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(recipient));
            email.Subject = subject;

            var builder = new BodyBuilder();


            builder.HtmlBody = body;

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration.GetSection("EmailHost").Value,
                587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration.GetSection("EmailUsername").Value,
                _configuration.GetSection("EmailPassword").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }


        public async Task<string> SendSMSAsync(string recipient, string message)
        {
            try
            {
                var apiUrl = _configuration.GetValue<string>("Nexmo:ApiUrl");
                var authToken = _configuration.GetValue<string>("Nexmo:AuthorizationToken");

                if (string.IsNullOrEmpty(apiUrl) || string.IsNullOrEmpty(authToken))
                {
                    return "Nexmo configuration is missing.";
                }

                var client = new RestClient(apiUrl);
                var request = new RestRequest("/sms/2/text/advanced", Method.Post);
                request.AddHeader("Authorization", authToken);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");

                var body = new
                {
                    messages = new[]
                    {
                        new
                        {
                            destinations = new[]
                            {
                                new { to = recipient }
                            },
                            from = "ServiceSMS",
                            text = message
                        }
                    }
                };

                request.AddJsonBody(body);

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    return "SMS sent successfully.";
                }
                else
                {
                    return $"Failed to send SMS. Status code: {response.StatusCode}, Error: {response.ErrorMessage}";
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send SMS: {ex.Message}");
            }
        }

        public async Task SendWhatsAppMessageAsync(string recipient, string message)
        {
            try
            {
                var accountSid = _configuration["Twilio:AccountSid"] ?? throw new InvalidOperationException("Twilio AccountSid is not found");
                var authToken = _configuration["Twilio:AuthToken"] ?? throw new InvalidOperationException("Twilio AuthToken is not found");
                var whatsAppNumber = _configuration["Twilio:WhatsAppNumber"] ?? throw new InvalidOperationException("Twilio WhatsAppNumber is not found");

                TwilioClient.Init(accountSid, authToken);

                var messageOptions = new CreateMessageOptions(
                    new PhoneNumber("whatsapp:+216" + recipient))
                {
                    From = new PhoneNumber(whatsAppNumber),
                    Body = message
                };

                await MessageResource.CreateAsync(messageOptions);

                // WhatsApp message sent successfully
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send WhatsApp message: {ex.Message}");
            }
        }
    }
}
