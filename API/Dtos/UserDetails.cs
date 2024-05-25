namespace API.Dtos
{
    public class UserDetails
    {
        public int Id { get; set; }

        public string Login { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Telephone { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; }
        public string Role { get; set; } = string.Empty;

        public bool Etat { get; set; }

        public string AvatarUrl { get; set; } = string.Empty;
    }
}
