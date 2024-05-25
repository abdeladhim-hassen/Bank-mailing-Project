using API.Dtos;
using API.Models;
using API.Services.CarteService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CarteController(ICarteService carteService) : ControllerBase
    {
        private readonly ICarteService _carteService = carteService;

        [HttpPost("add")]

        public async Task<ActionResult<ServiceResponse<int>>> AddCarte(CarteAdd carteAdd)
        {
            Carte carte = new()
            {

                Bin = carteAdd.Bin,
                DateDelivrance = DateTime.Now,
                DateExpiration = DateTime.Now.AddYears(3),
                Statut = carteAdd.Statut,
                PlafondGAB = carteAdd.PlafondGAB,
                PlafondTPE = carteAdd.PlafondTPE,
                ResteGAB = carteAdd.ResteGAB,
                ResteTPE = carteAdd.ResteTPE,
                Mobile = carteAdd.Mobile,
                ClientId = carteAdd.ClientId
            };

            var response = await _carteService.AddCarteAsync(carte);

            if (response.Data == 0)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        [HttpPut("update-status/{numeroCarte}")]
        public async Task<ActionResult<ServiceResponse<int>>> UpdateCarteStatus(int numeroCarte, bool newStatus)
        {
            ServiceResponse<int> response = await _carteService.UpdateCarteStatusAsync(numeroCarte, newStatus);

            if (response.Data == 0)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
