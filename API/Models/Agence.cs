
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

public partial class Agence
{
    [Key]
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    [InverseProperty("Agence")]
    public virtual ICollection<Client> Clients { get; set; } = [];
    [InverseProperty("Agence")]
    public virtual ICollection<User> Users { get; set; } = [];
}
