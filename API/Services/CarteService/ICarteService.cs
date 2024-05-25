using API.Dtos;
using API.Models;

namespace API.Services.CarteService
{
    public interface ICarteService
    {
        Task<ServiceResponse<int>> AddCarteAsync(Carte carte);
        Task<ServiceResponse<int>> UpdateCarteStatusAsync(int numeroCarte, bool newStatus);
    }
}
