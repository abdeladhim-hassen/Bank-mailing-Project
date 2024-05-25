
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;
public partial class Compte
{
    [Key]
    public int Id { get; set; }
    public int NumeroCompte { get; set; }
    public int ClientId { get; set; }

    [Column(TypeName = "decimal(18, 3)")]
    public decimal SoldeCompte { get; set; }

    public string TypeCompte { get; set; } = null!;

    public DateTime DateOuvertureCompte { get; set; }

    public string StatutCompte { get; set; } = string.Empty;

    [ForeignKey("ClientId")]
    [InverseProperty("Comptes")]
    public virtual Client Client { get; set; } = null!;

    [InverseProperty("Compte")]
    public virtual ICollection<Credit> Credits { get; set; } = [];
}

