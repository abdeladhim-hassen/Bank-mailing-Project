using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{

    public class UserLogin
    {
        [Required]
        public string Login { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}

