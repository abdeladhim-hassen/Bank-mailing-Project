using API.Dtos;

namespace API.Services.EventCreditConfigurationService
{
    public interface IEventCreditConfigurationService
    {
        Task<ServiceResponse<EvenementDto>> UpdateEventCreditAsync(EvenementDto request);
        Task<ServiceResponse<List<EvenementDto>>> GetAllEventCreditAsync();
    }
}
