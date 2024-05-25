using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace API.Models;
public partial class CartEvenement
{
    [Key]
    public int Id { get; set; }

    public string Canal { get; set; } = null!;

    public DateTime HeureEnvoi { get; set; }

    public int TemplateId { get; set; }

    public int CarteId { get; set; }

    [ForeignKey("CarteId")]
    [InverseProperty("CartEvenements")]
    public virtual Carte Carte { get; set; } = null!;

    [ForeignKey("TemplateId")]
    [InverseProperty("CartEvenements")]
    public virtual Template Template { get; set; } = null!;
}
