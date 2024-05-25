using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace API.Models;

public partial class User
{
    [Key]
    public int Id { get; set; }

    public string Login { get; set; } = string.Empty;

    public string Email { get; set; } =  string.Empty;

    public string FirstName { get; set; } =  string.Empty;

    public string LastName { get; set; } =  string.Empty;

    public string Telephone { get; set; } =  string.Empty;

    public byte[] PasswordHash { get; set; } = [];

    public byte[] PasswordSalt { get; set; } = [];

    public DateTime DateCreated { get; set; }

    public string VerificationCode { get; set; } =  string.Empty;

    public DateTime? VerificationCodeGeneratedAt { get; set; }

    public string Role { get; set; } =  string.Empty;

    public bool Etat { get; set; }

    public string AvatarUrl { get; set; } =  string.Empty;
    public int AgenceId { get; set; }
    [ForeignKey("AgenceId")]
    [InverseProperty("Users")]
    public virtual Agence Agence { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
