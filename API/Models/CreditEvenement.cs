
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;
public partial class CreditEvenement
{
    [Key]
    public int Id { get; set; }

    public string Canal { get; set; } = null!;

    public DateTime HeureEnvoi { get; set; }

    public int TemplateId { get; set; }

    public int CreditId { get; set; }

    [ForeignKey("CreditId")]
    [InverseProperty("CreditEvenements")]
    public virtual Credit Credit { get; set; } = null!;

    [ForeignKey("TemplateId")]
    [InverseProperty("CreditEvenements")]
    public virtual Template Template { get; set; } = null!;
}
