using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;


public partial class Credit
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "decimal(18, 3)")]
    public decimal MontantCredit { get; set; }

    [Column(TypeName = "decimal(18, 3)")]
    public decimal RestCredit { get; set; }

    [Column(TypeName = "decimal(18, 3)")]
    public decimal CreditMensuelle { get; set; }

    public DateTime LastVerificationDate { get; set; }

    public int JourPaiement { get; set; }
    public int CompteId { get; set; }
    [ForeignKey("CompteId")]
    [InverseProperty("Credits")]
    public virtual Compte Compte { get; set; } = null!;


    [InverseProperty("Credit")]
    public virtual ICollection<CreditEvenement> CreditEvenements { get; set; } = [];
}
