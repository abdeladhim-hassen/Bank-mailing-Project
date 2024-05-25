
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace API.Models;

public partial class Category
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } =  string.Empty;

    [InverseProperty("Category")]
    public virtual ICollection<Template> Templates { get; set; } = [];
}
