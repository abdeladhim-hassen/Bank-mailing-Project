
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;
public partial class Message
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; } =  null!;

    public DateTime Timestamp { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Messages")]
    public virtual User User { get; set; } = null!;
}
