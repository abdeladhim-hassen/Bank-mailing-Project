namespace API.Dtos
{
    public class TemplateDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public TemplateType TemplateType { get; set; }
        public string TemplateText { get; set; } = string.Empty;
    }
}
