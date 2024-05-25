using API.Dtos;
using API.Models;

namespace API.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Login(string Login, string password);
    }
}

