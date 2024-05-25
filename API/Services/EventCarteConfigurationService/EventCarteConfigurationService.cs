using API.Data;
using API.Dtos;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services.EventCarteConfigurationService
{
    public class EventCarteConfigurationService(DataContext context):IEventCarteConfigurationService
    {
        private readonly DataContext _context = context;
        public async Task<ServiceResponse<EvenementDto>> UpdateEventCarteAsync(EvenementDto request)
        {
            try
            {
                var carteEvent = await _context.CartEvenements.FirstOrDefaultAsync(c => c.Id == request.Id);
                if (carteEvent != null)
                {


                    carteEvent.HeureEnvoi = request.HeureEnvoi;
                    carteEvent.Canal = request.Canal;
                    // Save changes
                    await _context.SaveChangesAsync();
                    var cartEvenementDto = new EvenementDto
                    {
                        Id = carteEvent.Id,
                        Canal = carteEvent.Canal,
                        CategoryId = carteEvent.Template.CategoryId,
                        HeureEnvoi = carteEvent.HeureEnvoi,
                        Type = carteEvent.Template.Type,
                        NomClient = carteEvent.Carte.Client.Nom,
                        PrenomClient = carteEvent.Carte.Client.Prenom
                    };
                    return new ServiceResponse<EvenementDto> { Data = cartEvenementDto, Message = "Event updated successfully!" };

                }
                else
                {
                    return new ServiceResponse<EvenementDto> { Message = $"Carte not found" };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<EvenementDto> { Message = $"Failed to update Event : {ex.Message}" };
            }
        }
        public async Task<ServiceResponse<List<EvenementDto>>> GetAllEventCartAsync()
        {
            try
            {
                var cartEvenements = await _context.CartEvenements.ToListAsync();

                var cartEvenementDtos = cartEvenements.Select(cartEvenement => new EvenementDto
                {
                    Id = cartEvenement.Id,
                    CategoryId = cartEvenement.Template.CategoryId,
                    Canal = cartEvenement.Canal,
                    HeureEnvoi = cartEvenement.HeureEnvoi,
                    Type = cartEvenement.Template.Type, // Assuming you have a Type property in your MessageModel
                    NomClient = cartEvenement.Carte.Client.Nom,
                    PrenomClient = cartEvenement.Carte.Client.Prenom
                }).ToList();

                return new ServiceResponse<List<EvenementDto>>
                {
                    Data = cartEvenementDtos,
                    Message = "All Cart Evenements retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<EvenementDto>> { Message = $"Failed to retrieve Cart Evenements: {ex.Message}" };
            }
        }
    }
}
