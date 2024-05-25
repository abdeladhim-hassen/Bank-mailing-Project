using API.Dtos;

namespace API.Services.EventCarteConfigurationService
{
    public interface IEventCarteConfigurationService
    {
        Task<ServiceResponse<EvenementDto>> UpdateEventCarteAsync(EvenementDto request);
        Task<ServiceResponse<List<EvenementDto>>> GetAllEventCartAsync();
    }
}
