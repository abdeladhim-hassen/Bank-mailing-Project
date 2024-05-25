using API.Data;
using API.Dtos;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services.MessageModelService
{
    public class TemplateService(DataContext context) : ITemplateService
    {
        private readonly DataContext _context = context;
        public async Task<ServiceResponse<int>> UpdateTemplateAsync(TemplateDto templateUpdateDto)
        {
            try
            {
                var messageModel = await _context.Templates.FirstOrDefaultAsync(c => c.Id == templateUpdateDto.Id);
                if (messageModel != null)
                {
                    switch (templateUpdateDto.TemplateType)
                    {
                        case TemplateType.Email:
                            messageModel.EmailBody = templateUpdateDto.TemplateText;
                            break;
                        case TemplateType.SMS:
                            messageModel.SMSMessage = templateUpdateDto.TemplateText;
                            break;
                        case TemplateType.WhatsApp:
                            messageModel.WhatsMessage = templateUpdateDto.TemplateText;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(templateUpdateDto.TemplateType), templateUpdateDto.TemplateType, "Invalid template type specified.");;
                    }

                    // Save changes
                    await _context.SaveChangesAsync();

                    return new ServiceResponse<int>
                    {
                        Data = messageModel.Id,
                        Message = $"{templateUpdateDto.TemplateType} Template updated successfully!"
                    };
                }
                else
                {
                    return new ServiceResponse<int> { Message = $"MessageModel not found" };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<int> { Message = $"Failed to update {templateUpdateDto.TemplateType} Template: {ex.Message}" };
            }
        }



        public async Task<ServiceResponse<List<TemplateDto>>> GetAllTemplateAsync(int categoryId,TemplateType templateType)
        {
            try
            {
                var messageModels = await _context.Templates.Where(t => t.CategoryId == categoryId).ToListAsync();


                if (messageModels.Count != 0)
                {
                    var templateDtos = messageModels.Select(messageModel => new TemplateDto
                    {
                        Id = messageModel.Id,
                        Name = messageModel.Type,
                        CategoryId = messageModel.CategoryId,
                        TemplateType = templateType,
                        TemplateText = GetTemplateSelector(templateType, messageModel)
                    }).ToList();

                    return new ServiceResponse<List<TemplateDto>>
                    {
                        Data = templateDtos,
                        Message = $"{templateType} Templates retrieved successfully!"
                    };
                }
                else
                {
                    return new ServiceResponse<List<TemplateDto>> { Message = $"No {templateType} Templates found" };
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                return new ServiceResponse<List<TemplateDto>> { Message = $"Failed to retrieve {templateType} Templates: {ex.Message}" };
            }
        }

        private static string GetTemplateSelector(TemplateType templateType, Template messageModel)
        {
            return templateType switch
            {
                TemplateType.Email => messageModel.EmailBody,
                TemplateType.SMS => messageModel.SMSMessage,
                TemplateType.WhatsApp => messageModel.WhatsMessage,
                _ => throw new ArgumentOutOfRangeException(nameof(templateType), templateType, null)
            };
        }



    }
}
