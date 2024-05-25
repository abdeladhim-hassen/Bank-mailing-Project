using API.Dtos;

namespace API.Services.MessageModelService
{
    public interface ITemplateService
    {
        Task<ServiceResponse<int>> UpdateTemplateAsync(TemplateDto TemplateUpdateDto);
        Task<ServiceResponse<List<TemplateDto>>> GetAllTemplateAsync(int categoryId, TemplateType templateType);
    }
}
