using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Models;
public partial class Template
{
    [Key]
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string Type { get; set; } =  string.Empty;

    public string EmailBody { get; set; } =  string.Empty;
    public string SMSMessage { get; set; } =  string.Empty;

    public string WhatsMessage { get; set; } =  string.Empty;

    [InverseProperty("Template")]
    public virtual ICollection<CartEvenement> CartEvenements { get; set; } = new List<CartEvenement>();

    [ForeignKey("CategoryId")]
    [InverseProperty("Templates")]
    public virtual Category Category { get; set; } =  null!;

    [InverseProperty("Template")]
    public virtual ICollection<CreditEvenement> CreditEvenements { get; set; } = [];
}
