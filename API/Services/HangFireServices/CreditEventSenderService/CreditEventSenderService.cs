using API.Data;
using API.Models;
using API.Services.CommunicationService;
using Microsoft.EntityFrameworkCore;

namespace API.Services.HangFireServices.CreditEventSenderService
{
    public class CreditEventSenderService(DataContext context,
                                    ILogger<CreditEventSenderService> logger,
                                    ICommunicationService communicationService) : ICreditEventSenderService
    {

        private readonly DataContext _context = context;
        private readonly ICommunicationService _communicationService = communicationService;
        private readonly ILogger<CreditEventSenderService> _logger = logger;
        public async Task SendingCreditEventAsync()
        {
            try
            {
                // Retrieve all CreditEvenement from the database
                var currentDate = DateTime.Now;
                var creditEvenements = await _context.CreditEvenements
                    .Where(x => x.HeureEnvoi.Year == currentDate.Year &&
                                x.HeureEnvoi.Month == currentDate.Month &&
                                x.HeureEnvoi.Day == currentDate.Day &&
                                x.HeureEnvoi.Hour == currentDate.Hour
                          )
                    .ToListAsync();

                foreach (var creditEvenement in creditEvenements)
                {
                    bool success = false;
                    switch (creditEvenement.Canal.ToUpper())
                    {
                        case "EMAIL":
                            try
                            {
                                await _communicationService.SendEmailAsync(
                                    recipient: creditEvenement.Credit.Compte.Client.AdresseEmail,
                                    subject: creditEvenement.Template.Type,
                                    body: BuildMessage(creditEvenement.Template.EmailBody, creditEvenement.Credit));
                                success = true;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning($"Failed to send email for CreditEvenement ID: {creditEvenement.Id}, Error: {ex.Message}");
                            }
                            break;
                        case "SMS":
                            try
                            {
                                var result = await _communicationService.SendSMSAsync(
                                    recipient: creditEvenement.Credit.Compte.Client.NumeroTelephone,
                                    message: BuildMessage(creditEvenement.Template.SMSMessage, creditEvenement.Credit));
                                if (result.Contains("successfully"))
                                {
                                    success = true;
                                }
                                else
                                {
                                    _logger.LogWarning($"Failed to send SMS for CreditEvenement ID: {creditEvenement.Id}, Error: {result}");
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning($"Failed to send SMS for CreditEvenement ID: {creditEvenement.Id}, Error: {ex.Message}");
                            }
                            break;
                        case "WHATSAPP":
                            try
                            {
                                await _communicationService.SendWhatsAppMessageAsync(
                                    recipient: creditEvenement.Credit.Compte.Client.NumeroTelephone,
                                    message: BuildMessage(creditEvenement.Template.WhatsMessage, creditEvenement.Credit));
                                success = true;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning($"Failed to send WhatsApp message for CreditEvenement ID: {creditEvenement.Id}, Error: {ex.Message}");
                            }
                            break;
                        default:
                            _logger.LogWarning($"Unknown channel '{creditEvenement.Canal}' for CreditEvenement ID: {creditEvenement.Id}");
                            break;
                    }

                    if (success)
                    {
                        // Remove the processed CreditEvenement from the database
                        _context.CreditEvenements.Remove(creditEvenement);
                    }
                }

                // Save changes to the database (remove the processed CreditEvenement)
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                _logger.LogWarning($"An error occurred: {ex.Message}");
            }
        }
        private static string BuildMessage(string template, Credit CreditInfo)
        {
            string messageContent = template;

            // Replace hashtags with client information if they exist
            messageContent = messageContent.Replace("#[Nom Agence]", CreditInfo.Compte.Client.Agence.Nom);
            messageContent = messageContent.Replace("#[Numero du Compte]", CreditInfo.Compte.NumeroCompte.ToString());
            messageContent = messageContent.Replace("#[Montant Mensuelle]", CreditInfo.CreditMensuelle.ToString("yyyy/MM/dd"));
            messageContent = messageContent.Replace("#[Jour de paiement]", CreditInfo.JourPaiement.ToString());
            messageContent = messageContent.Replace("#[Nom Client]", CreditInfo.Compte.Client.Nom);
            messageContent = messageContent.Replace("#[Prenom Client]", CreditInfo.Compte.Client.Prenom);

            return messageContent;
        }


    }
}
