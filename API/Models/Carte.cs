using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace API.Models;
public partial class Carte
{
    [Key]
    public int NumeroCarte { get; set; }

    public int Bin { get; set; }

    public DateTime DateDelivrance { get; set; }

    public DateTime DateExpiration { get; set; }

    public bool Statut { get; set; }

    public int PlafondGAB { get; set; }

    public int PlafondTPE { get; set; }

    public int ResteGAB { get; set; }

    public int ResteTPE { get; set; }

    public int Mobile { get; set; }

    public int ClientId { get; set; }

    [InverseProperty("Carte")]
    public virtual ICollection<CartEvenement> CartEvenements { get; set; } = [];

    [ForeignKey("ClientId")]
    [InverseProperty("Cartes")]
    public virtual Client Client { get; set; } = null!;
}
