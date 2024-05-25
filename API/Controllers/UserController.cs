using API.Dtos;
using API.Models;
using API.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("add"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<int>>> AddUser(UserRegister request)
        {
            User user = new()
            {
                Login = request.Login,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Telephone = request.Telephone,
                Email = request.Email,
                Role = request.Role,
                Etat = request.Etat,
                AvatarUrl = request.AvatarUrl,
                AgenceId = request.AgenceId,
            };


            ServiceResponse<int> response = await _userService.AddUserAsync(user);

            if (response.Data == 0)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<UserDetails>>> EditUser([FromBody] UserUpdate request)
        {
            try
            {
                ServiceResponse<UserDetails> response = await _userService.EditUserAsync(request);

                if (response.Data == null)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ServiceResponse<UserDetails> { Message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<UserDetails>>>> GetAllUsers()
        {
            ServiceResponse<List<UserDetails>> response = await _userService.GetAllUsersAsync();
            if (response.Data != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("change-password"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword(UserChangePassword userChangePassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest(new ServiceResponse<bool> { Message = "User ID not found." });
            }

            var response = await _userService.ChangePassword(int.Parse(userId), userChangePassword.Password);

            if (!response.Data)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("reset-password")]
        public async Task<ActionResult<ServiceResponse<bool>>> ResetPasswordWithVerificationCode(ResetPassword resetPasswordRequest)
        {
            var response = await _userService.ResetPasswordWithVerificationCode(resetPasswordRequest.Email, resetPasswordRequest.VerificationCode, resetPasswordRequest.Password);

            if (!response.Data)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("send-verification-code")]
        public async Task<ActionResult<ServiceResponse<bool>>> SendVerificationCode(string Email)
        {
            var response = await _userService.SendVerificationCode(Email);

            if (!response.Data)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("check-email-exists/{email}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            bool userExists = await _userService.UserEmailExists(email);
            return Ok(userExists);
        }

        [HttpGet("check-login-exists/{login}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> CheckLoginExists(string login)
        {
            bool userExists = await _userService.UserLoginExists(login);
            return Ok(userExists);
        }
    }
}
