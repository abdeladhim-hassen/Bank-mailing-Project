using API.Data;
using API.Models;
using API.Services.CommunicationService;
using Microsoft.EntityFrameworkCore;

namespace API.Services.HangFireServices.CartEventSenderService
{
    public class CartEventSenderService(DataContext context,
                                    ILogger<CartEventSenderService> logger,
                                    ICommunicationService communicationService) : ICartEventSenderService
    {

        private readonly DataContext _context = context;
        private readonly ICommunicationService _communicationService = communicationService;
        private readonly ILogger<CartEventSenderService> _logger = logger;
        public async Task SendingCarteEventAsync()
        {
            try
            {
                // Retrieve all CartEvenements from the database
                var currentDate = DateTime.Now;
                var cartEvenements = await _context.CartEvenements
                    .Where(x => x.HeureEnvoi.Year == currentDate.Year &&
                                x.HeureEnvoi.Month == currentDate.Month &&
                                x.HeureEnvoi.Day == currentDate.Day &&
                                x.HeureEnvoi.Hour == currentDate.Hour
                          )
                    .ToListAsync();

                foreach (var cartEvenement in cartEvenements)
                {
                    bool success = false;
                    switch (cartEvenement.Canal.ToUpper())
                    {
                        case "EMAIL":
                            try
                            {
                                await _communicationService.SendEmailAsync(
                                    recipient: cartEvenement.Carte.Client.AdresseEmail,
                                    subject: cartEvenement.Template.Type,
                                    body: BuildMessage(cartEvenement.Template.EmailBody, cartEvenement.Carte));
                                success = true;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning($"Failed to send email for CartEvenement ID: {cartEvenement.Id}, Error: {ex.Message}");
                            }
                            break;
                        case "SMS":
                            try
                            {
                                var result = await _communicationService.SendSMSAsync(
                                    recipient: cartEvenement.Carte.Client.NumeroTelephone,
                                    message: BuildMessage(cartEvenement.Template.SMSMessage, cartEvenement.Carte));
                                if (result.Contains("successfully"))
                                {
                                    success = true;
                                }
                                else
                                {
                                    _logger.LogWarning($"Failed to send SMS for CartEvenement ID: {cartEvenement.Id}, Error: {result}");
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning($"Failed to send SMS for CartEvenement ID: {cartEvenement.Id}, Error: {ex.Message}");
                            }
                            break;
                        case "WHATSAPP":
                            try
                            {
                                await _communicationService.SendWhatsAppMessageAsync(
                                    recipient: cartEvenement.Carte.Client.NumeroTelephone,
                                    message: BuildMessage(cartEvenement.Template.WhatsMessage, cartEvenement.Carte));
                                success = true;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning($"Failed to send WhatsApp message for CartEvenement ID: {cartEvenement.Id}, Error: {ex.Message}");
                            }
                            break;
                        default:
                            _logger.LogWarning($"Unknown channel '{cartEvenement.Canal}' for CartEvenement ID: {cartEvenement.Id}");
                            break;
                    }

                    if (success)
                    {
                        // Remove the processed CartEvenement from the database
                        _context.CartEvenements.Remove(cartEvenement);
                    }
                }

                // Save changes to the database (remove the processed CartEvenements)
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                _logger.LogWarning($"An error occurred: {ex.Message}");
            }
        }
        private static string BuildMessage(string template, Carte CartInfo)
        {
            string messageContent = template;

            // Replace hashtags with client information if they exist
            messageContent = messageContent.Replace("#[Nom Agence]", CartInfo.Client.Agence.Nom);
            messageContent = messageContent.Replace("#[Numero de Carte]", CartInfo.NumeroCarte.ToString());
            messageContent = messageContent.Replace("#[Date Expiration]", CartInfo.DateExpiration.ToString("yyyy/MM/dd"));
            messageContent = messageContent.Replace("#[Nom Client]", CartInfo.Client.Nom);
            messageContent = messageContent.Replace("#[Prenom Client]", CartInfo.Client.Prenom);

            return messageContent;
        }


    }
}
