using API.Data;
using API.Dtos;
using Microsoft.EntityFrameworkCore;

namespace API.Services.EventCreditConfigurationService
{
    public class EventCreditConfigurationService(DataContext context) : IEventCreditConfigurationService
    {
        private readonly DataContext _context = context;

        public async Task<ServiceResponse<EvenementDto>> UpdateEventCreditAsync(EvenementDto request)
        {
            var response = new ServiceResponse<EvenementDto>();

            try
            {
                var creditEvent = await _context.CreditEvenements.FirstOrDefaultAsync(c => c.Id == request.Id);

                if (creditEvent == null)
                {
                    response.Message = "Credit event not found";
                    return response;
                }

                creditEvent.HeureEnvoi = request.HeureEnvoi;
                creditEvent.Canal = request.Canal;

                // Save changes
                await _context.SaveChangesAsync();

                var updatedCreditEventDto = new EvenementDto
                {
                    Id = creditEvent.Id,
                    Canal = creditEvent.Canal,
                    CategoryId = creditEvent.Template.CategoryId,
                    HeureEnvoi = creditEvent.HeureEnvoi,
                    Type = creditEvent.Template.Type,
                    NomClient = creditEvent.Credit.Compte.Client.Nom,
                    PrenomClient = creditEvent.Credit.Compte.Client.Prenom
                };

                response.Data = updatedCreditEventDto;
                response.Message = "Event updated successfully!";
            }
            catch (Exception ex)
            {
                response.Message = $"Failed to update event: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<List<EvenementDto>>> GetAllEventCreditAsync()
        {
            var response = new ServiceResponse<List<EvenementDto>>();

            try
            {
                var creditEvenements = await _context.CreditEvenements.ToListAsync();

                var creditEvenementDtos = creditEvenements.Select(creditEvenement => new EvenementDto
                {
                    Id = creditEvenement.Id,
                    CategoryId = creditEvenement.Template.CategoryId,
                    Canal = creditEvenement.Canal,
                    HeureEnvoi = creditEvenement.HeureEnvoi,
                    Type = creditEvenement.Template.Type,
                    NomClient = creditEvenement.Credit.Compte.Client.Nom,
                    PrenomClient = creditEvenement.Credit.Compte.Client.Prenom
                }).ToList();

                response.Data = creditEvenementDtos;
                response.Message = "All Credit Evenements retrieved successfully";
            }
            catch (Exception ex)
            {
                response.Message = $"Failed to retrieve Credit Evenements: {ex.Message}";
            }

            return response;
        }
    }
}
