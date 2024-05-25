using API.Data;
using API.Dtos;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Services.AuthService
{
    public class AuthService(DataContext context,
        IConfiguration configuration) : IAuthService
    {
        private readonly DataContext _context = context;
        private readonly IConfiguration _configuration = configuration;
        public async Task<ServiceResponse<string>> Login(string Login, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Login.Equals(Login));
            if (user == null)
            {

                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {

                response.Message = "Wrong password.";
            }
            else
            {
                response.Data = CreateToken(user);
            }

            return response;
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash =
                hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Role, user.Role),
                new("Role", user.Role),
                new("AvatarUrl", user.AvatarUrl),
                new("FirstName", user.FirstName),
                new("LastName", user.LastName),
                new("Email", user.Email),
                new("UserName", user.Login),
            };

            var tokenKey = _configuration["AppSettings:Token"] ?? throw new InvalidOperationException("secret key is not found");
            // Retrieve the token key from appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
