namespace API.Dtos
{
    public class EvenementDto
    {
        public int Id { get; set; }
        public string Canal { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public DateTime HeureEnvoi { get; set; }
        public string Type { get; set; } = string.Empty;
        public string NomClient { get; set; } = string.Empty;
        public string PrenomClient { get; set; } = string.Empty;
    }
}
