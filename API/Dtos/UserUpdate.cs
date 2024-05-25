using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class UserUpdate
    {
        public int Id { get; set; }
        public string? AvatarUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le champ 'Nom' est requis.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères.")]
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le numéro de téléphone est requis.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Le format du numéro de téléphone n'est pas valide. Veuillez saisir 8 chiffres.")]
        public string Telephone { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public bool Etat { get; set; }
    }
}
