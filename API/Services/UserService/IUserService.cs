using API.Dtos;
using API.Models;

namespace API.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<int>> AddUserAsync(User user);
        Task<ServiceResponse<UserDetails>> EditUserAsync(UserUpdate userUpdate);
        Task<ServiceResponse<List<UserDetails>>> GetAllUsersAsync();
        Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword);
        Task<ServiceResponse<bool>> SendVerificationCode(string email);
        Task<ServiceResponse<bool>> ResetPasswordWithVerificationCode(string email, string verificationCode, string newPassword);
        Task<User?> GetUserByLogin(string login);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(int Id);
        Task<bool> UserLoginExists(string login);
        Task<bool> UserEmailExists(string email);
    }
}
