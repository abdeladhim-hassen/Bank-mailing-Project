using API.Data;
using API.Dtos;
using Microsoft.EntityFrameworkCore;

namespace API.Services.AgenceService
{
    public class AgenceService(DataContext context) : IAgenceService
    {
        private readonly DataContext _context = context;
        public async Task<ServiceResponse<List<AgenceDetails>>> GetAllAgencesAsync()
        {
            try
            {
                var agences = await _context.Agences
                    .Select(u => new AgenceDetails
                    {
                        Id = u.Id,
                        Nom = u.Nom,
                    })
                    .ToListAsync();

                return new ServiceResponse<List<AgenceDetails>>
                {
                    Data = agences,
                    Message = "Users fetched successfully!"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<AgenceDetails>>
                {
                    Data = null,
                    Message = $"Error fetching users: {ex.Message}"
                };
            }
        }
    }
}
