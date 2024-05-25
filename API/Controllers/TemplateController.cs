using API.Dtos;
using API.Services.MessageModelService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TemplateController(ITemplateService templateService) : ControllerBase
    {
        private readonly ITemplateService _templateService = templateService;

        [HttpPut("UpdateTemplate/")]
        public async Task<IActionResult> UpdateTemplate(TemplateDto templateUpdateDto)
        {
            ServiceResponse<int> response = await _templateService.UpdateTemplateAsync(templateUpdateDto);
            if (response.Data != 0)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpGet("templates/{categoryId}")]
        public async Task<ActionResult<ServiceResponse<List<TemplateDto>>>> GetAllTemplateAsync(int categoryId, [FromQuery] TemplateType templateType)
        {
            ServiceResponse<List<TemplateDto>> response = await _templateService.GetAllTemplateAsync(categoryId,templateType);
            if (response.Data != null)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
