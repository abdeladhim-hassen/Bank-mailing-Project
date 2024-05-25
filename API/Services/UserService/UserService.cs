using API.Data;
using API.Dtos;
using API.Models;
using API.Services.CommunicationService;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Services.UserService
{
    public class UserService(DataContext context, 
        ICommunicationService communicationService): IUserService
    {
        private readonly DataContext _context = context;
        private readonly ICommunicationService _communicationService = communicationService;
        public async Task<ServiceResponse<int>> AddUserAsync(User user)
        {
            if (await UserLoginExists(user.Login) || await UserEmailExists(user.Email))
            {
                return new ServiceResponse<int>
                {
                    Message = "User already exists."
                };
            }

            var password = GenerateRandomPassword();
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;


            // Begin a database transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var emailUser = new UserForEmail
                {
                    Login = user.Login,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = password
                };

                await SendPasswordByEmailAsync(emailUser);

                // If both database operations succeed, commit the transaction
                await transaction.CommitAsync();

                return new ServiceResponse<int> { Data = user.Id, Message = "Registration successful!" };
            }
            catch (Exception ex)
            {
                // If any operation fails, rollback the transaction to undo the user addition
                await transaction.RollbackAsync();
                Console.WriteLine($"Failed to add user: {ex.Message}");
                return new ServiceResponse<int> { Message = "Failed to add user. Please try again later." };
            }
        }

        public async Task<ServiceResponse<UserDetails>> EditUserAsync(UserUpdate userUpdate)
        {
   
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userUpdate.Id);

                if (existingUser == null)
                {
                    return new ServiceResponse<UserDetails>
                    {
                        Message = "User not found."
                    };
                }

                existingUser.FirstName = userUpdate.FirstName;
                existingUser.LastName = userUpdate.LastName;
                existingUser.Telephone = userUpdate.Telephone;
                existingUser.Etat = userUpdate.Etat;
                if(existingUser.Role != "Admin")
                    {
                        existingUser.Role = userUpdate.Role;
                    }
                if (!string.IsNullOrEmpty(userUpdate.AvatarUrl))
                {
                    existingUser.AvatarUrl = userUpdate.AvatarUrl;
                }

                await _context.SaveChangesAsync();

                var userDetails = new UserDetails
                {
                    Id = existingUser.Id,
                    Login = existingUser.Login,
                    Email = existingUser.Email,
                    FirstName = existingUser.FirstName,
                    LastName = existingUser.LastName,
                    Telephone = existingUser.Telephone,
                    DateCreated = existingUser.DateCreated,
                    Role = existingUser.Role,
                    Etat = existingUser.Etat,
                    AvatarUrl = existingUser.AvatarUrl
                };

                return new ServiceResponse<UserDetails> { Data = userDetails, Message = "User updated successfully!" };
            
        }


        public async Task<ServiceResponse<List<UserDetails>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Users
                    .Select(u => new UserDetails
                    {
                        Id = u.Id,
                        Login = u.Login,
                        Email = u.Email,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Telephone = u.Telephone,
                        DateCreated = u.DateCreated,
                        Role = u.Role,
                        Etat = u.Etat,
                        AvatarUrl = u.AvatarUrl
                    })
                    .ToListAsync();

                return new ServiceResponse<List<UserDetails>>
                {
                    Data = users,
                    Message = "Users fetched successfully!"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<UserDetails>>
                {
                    Data = null,
                    Message = $"Error fetching users: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<bool>
                {
                    Message = "User not found."
                };
            }

            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true, Message = "Password has been changed." };
        }

        public async Task<ServiceResponse<bool>> SendVerificationCode(string email)
        {
            var user = await GetUserByEmail(email);
            if (user == null)
            {
                return new ServiceResponse<bool>
                {
                    Message = "User not found."
                };
            }

            string verificationCode = GenerateVerificationCode();

           
            user.VerificationCode = verificationCode;
            user.VerificationCodeGeneratedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();


            await SendVerificationCodeEmail(email, verificationCode);

            return new ServiceResponse<bool>
            {
                Data = true,
                Message = "Verification code has been sent to your email. Please enter the code to verify before resetting your password."
            };
        }

        public async Task<ServiceResponse<bool>> ResetPasswordWithVerificationCode(string email, string verificationCode, string newPassword)
        {
            var user = await GetUserByEmail(email);
            if (user == null)
            {
                return new ServiceResponse<bool>
                {
                    Message = "User not found."
                };
            }

            if ((user.VerificationCodeGeneratedAt?.AddMinutes(3) < DateTime.UtcNow) || (user.VerificationCode != verificationCode))

            {
                return new ServiceResponse<bool>
                {
                    Message = "Invalid or expired verification code."
                };
            }

            user.VerificationCode = string.Empty;
            user.VerificationCodeGeneratedAt = null;

            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Data = true,
                Message = "Password has been changed successfully."
            };
        }

        public async Task<User?> GetUserById(int Id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == Id );
        }
        public async Task<User?> GetUserByLogin(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login.Equals(login));
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static string GenerateRandomPassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@";
            var random = new Random();

            int passwordLength = random.Next(10, 16); 
            char[] password = new char[passwordLength];

            password[0] = validChars[random.Next(validChars.Length)];
            password[1] = validChars[random.Next(validChars.Length)];
            password[2] = validChars[random.Next(validChars.Length)];
            password[3] = '@'; 

            for (int i = 4; i < passwordLength; i++)
            {
                password[i] = validChars[random.Next(validChars.Length)];
            }

            for (int i = passwordLength - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (password[i], password[j]) = (password[j], password[i]);
            }

            return new string(password);
        }

        public async Task<bool> UserLoginExists(string login)
        {
            return await _context.Users.AnyAsync(user => user.Login.Equals(login));
        }

        public async Task<bool> UserEmailExists(string email)
        {
            return await _context.Users.AnyAsync(user => user.Email.Equals(email));
        }

        private static string GenerateVerificationCode()
        {
            Random random = new();
            int verificationCode = random.Next(100000, 999999);
            return verificationCode.ToString();
        }

        private async Task SendPasswordByEmailAsync(UserForEmail user)
        {
            string emailSubject = "Your New Account Details";
            string emailBody = $@"<!DOCTYPE html>
                <html lang=""en"">

                <head>
                    <title></title>
                    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <style>
                        * {{
                            box-sizing: border-box;
                        }}

                        body {{
                            margin: 0;
                            padding: 0;
                            -webkit-text-size-adjust: none;
                            text-size-adjust: none;
                            background-color: #FFFFFF;
                        }}

                        a[x-apple-data-detectors] {{
                            color: inherit !important;
                            text-decoration: none !important;
                        }}

                        #MessageViewBody a {{
                            color: inherit;
                            text-decoration: none;
                        }}

                        p {{
                            line-height: inherit;
                            margin: 0;
                        }}

                        @media (max-width:520px) {{
                            .desktop_hide,
                            .desktop_hide table {{
                                display: none !important;
                            }}

                            .image_block img+div {{
                                display: none;
                            }}

                            .mobile_hide {{
                                display: none;
                                min-height: 0;
                                max-height: 0;
                                max-width: 0;
                                overflow: hidden;
                                font-size: 0px;
                            }}
                        }}
                    </style>
                </head>

                <body>
                    <table class=""nl-container"" width=""100%"" border=""0"" cellpadding=""0"" 
                        cellspacing=""0"" role=""presentation"">
                        <tbody>
                            <tr>
                                <td>
                                    <table class=""row row-1"" align=""center"" width=""100%""
                                       border=""0"" cellpadding=""0"" cellspacing=""0"" 
                                        role=""presentation"">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <table class=""row-content stack""
                                                        align=""center"" border=""0"" cellpadding=""0""
                                                        cellspacing=""0"" role=""presentation""
                                                        style=""background-color: #ffffff;
                                                        color: #000000; width: 500px; margin: 0 auto;""
                                                        width=""500"">
                                                        <tbody>
                                                            <tr>
                                                                <td class=""column column-1""
                                                                width=""100%"" style=""font-weight: 400;
                                                                text-align: left; padding-bottom: 20px; padding-top: 15px;
                                                                vertical-align: top;"">
                                                                    <table class=""image_block 
                                                                    block-1"" width=""100%"" border=""0"" cellpadding=""0""
                                                                    cellspacing=""0"" role=""presentation"">
                                                                        <tr>
                                                                            <td class=""pad"" style=""padding-bottom:5px;padding-left:5px;
                                                                             padding-right:5px;width:100%;"">
                                                                                <div class=""alignment"" align=""center"">
                                                                                    <div class=""fullWidth"" style=""max-width: 350px;"">
                                                                                        <img src=""https://d1oco4z2z1fhwp.cloudfront.net/templates/default/2966/gif-resetpass.gif"" 
                                                                                        style=""display: block; height: auto; border: 0; width: 100%;"" width=""350"" 
                                                                                        alt=""reset-password"" title=""reset-password"" height=""auto""></div>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table class=""heading_block block-2"" width=""100%"" border=""0"" 
                                                                      cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                                                                        <tr>
                                                                            <td class=""pad"" style=""text-align:center;width:100%;"">
                                                                                <h1 style=""margin: 0; color: #393d47; font-family: Tahoma, Verdana,
                                                                                Segoe, sans-serif; font-size: 25px; font-weight: normal; 
                                                                                letter-spacing: normal; line-height: 120%; text-align: center;"">
                                                                                <strong>Your New Account Details</strong></h1>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table class=""paragraph_block block-3"" width=""100%"" border=""0"" 
                                                                        cellpadding=""10"" cellspacing=""0"" role=""presentation"" 
                                                                        style=""word-break: break-word;"">
                                                                        <tr>
                                                                            <td class=""pad"">
                                                                                <div style=""color:#393d47;font-family:Tahoma,Verdana,
                                                                                 Segoe,sans-serif;font-size:14px;line-height:150%;text-align:center;"">
                                                                                    <p>Hi {user.FirstName} {user.LastName},</p>
                                                                                    <p>We are pleased to inform you that your account has been successfully created. Below are your login credentials:</p>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table class=""button_block block-4"" width=""100%"" border=""0""
                                                                     cellpadding=""15"" cellspacing=""0"" role=""presentation"">
                                                                        <tr>
                                                                            <td class=""pad"">
                                                                                <div class=""alignment"" align=""center""><span style=""display:inline-block;color:#FFFFFF;
                                                                                background-color:#ffc727;border-radius:5px;width:200px;padding-top:10px;
                                                                                padding-bottom:10px;font-family:Tahoma, Verdana, Segoe, sans-serif;
                                                                                font-size:18px;text-align:center;"">{user.Login}</span></div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table class=""button_block block-5"" width=""100%"" border=""0"" 
                                                                        cellpadding=""15"" cellspacing=""0"" role=""presentation"">
                                                                        <tr>
                                                                            <td class=""pad"">
                                                                                <div class=""alignment"" align=""center""><span style=""display:inline-block;
                                                                                    color:#FFFFFF;background-color:#ffc727;border-radius:5px;width:200px;
                                                                                    padding-top:10px;padding-bottom:10px;font-family:Tahoma, Verdana, Segoe, sans-serif;
                                                                                    font-size:18px;text-align:center;"">{user.Password}</span></div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </body>

                </html>";

            await _communicationService.SendEmailAsync(user.Email, emailSubject, emailBody);
        }

        private async Task SendVerificationCodeEmail(string email, string verificationCode)
        {
            string emailSubject = "Verification Code for Password Reset";
            string emailBody = $"Your verification code is: {verificationCode}";

            await _communicationService.SendEmailAsync(email, emailSubject, emailBody);
        }
    }
}
