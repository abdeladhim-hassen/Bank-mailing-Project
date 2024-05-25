using API.Dtos;

namespace API.Services.AgenceService
{
    public interface IAgenceService
    {
        Task<ServiceResponse<List<AgenceDetails>>> GetAllAgencesAsync();
    }
}
