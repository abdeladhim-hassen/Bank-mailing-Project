
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace API.Models;

public partial class Client
{
    [Key]
    public int Id { get; set; }

    public string Nom { get; set; } =  string.Empty;

    public string Prenom { get; set; } =  string.Empty;

    public string Adresse { get; set; } =  string.Empty;

    public string NumeroTelephone { get; set; } =  string.Empty;

    public string AdresseEmail { get; set; } =  string.Empty;

    public DateTime DateNaissance { get; set; }

    public int AgenceId { get; set; }

    public string CanalPrefere { get; set; } =  string.Empty;

    [ForeignKey("AgenceId")]
    [InverseProperty("Clients")]
    public virtual Agence Agence { get; set; } = null!;

    [InverseProperty("Client")]
    public virtual ICollection<Carte> Cartes { get; set; } = [];

    [InverseProperty("Client")]
    public virtual ICollection<Compte> Comptes { get; set; } = [];

    [InverseProperty("Client")]
    public virtual ICollection<ClientNotification> ClientNotifications { get; set; } = [];
}
