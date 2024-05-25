using API.Data;
using API.Dtos;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services.CarteService
{
    public class CarteService(DataContext context) : ICarteService
    {
        private readonly DataContext _context = context;

        public async Task<ServiceResponse<int>> AddCarteAsync(Carte carte)
        {
            if (await NumeroCarteExists(carte.NumeroCarte))
            {
                return new ServiceResponse<int>
                {
                    Message = "NumeroCarte already exists."
                };
            }

            // Disable all triggers on the Cartes table
            await _context.Database.ExecuteSqlRawAsync("DISABLE TRIGGER ALL ON Cartes");

            _context.Cartes.Add(carte);
            await _context.SaveChangesAsync();

            // Add CartEvenement entry
            await AddCartEvenementAsync(carte, 1); // Assuming MessageId 1 for carte insertion

            // Enable all triggers on the Cartes table
            await _context.Database.ExecuteSqlRawAsync("ENABLE TRIGGER ALL ON Cartes");

            return new ServiceResponse<int> { Data = carte.NumeroCarte, Message = "Carte added successfully!" };
        }

        public async Task<bool> NumeroCarteExists(int NumeroCarte)
        {
            if (await _context.Cartes.AnyAsync(carte => carte.NumeroCarte
                 .Equals(NumeroCarte)))
            {
                return true;
            }
            return false;
        }


        public async Task<ServiceResponse<int>> UpdateCarteStatusAsync(int numeroCarte, bool newStatus)
        {
            try
            {
                var carte = await _context.Cartes.FirstOrDefaultAsync(c => c.NumeroCarte == numeroCarte);
                if (carte != null)
                {
                    if (carte.Statut != newStatus)
                    {
                        carte.Statut = newStatus;

                        // Disable all triggers on the Cartes table
                        await _context.Database.ExecuteSqlRawAsync("DISABLE TRIGGER ALL ON Cartes");

                        // Save changes
                        await _context.SaveChangesAsync();
                        // Add CartEvenement entry
                        await AddCartEvenementAsync(carte, newStatus ? 2 : 3); // Assuming MessageId 2 for status active and 3 for status blocked
                                                                               // Enable all triggers on the Cartes table
                        await _context.Database.ExecuteSqlRawAsync("ENABLE TRIGGER ALL ON Cartes");
                        return new ServiceResponse<int> { Data = carte.NumeroCarte, Message = "Statut updated successfully!" };
                    }
                    else
                    {
                        return new ServiceResponse<int> { Data = carte.NumeroCarte, Message = (carte.Statut == true) ? "Carte Status is already Active" : "Carte Status is already Bloqué" };
                    }
                }
                else
                {
                    return new ServiceResponse<int> { Message = $"Carte not found" };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<int> { Message = $"Failed to update Carte status: {ex.Message}" };
            }
        }

        private async Task AddCartEvenementAsync(Carte carte, int messageId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == carte.ClientId);
            var canalPrefere = client != null ? client.CanalPrefere : "SMS"; // Assuming default is SMS

            var cartEvenement = new CartEvenement
            {
                Canal = canalPrefere,
                HeureEnvoi = DateTime.Now.AddHours(1),
                TemplateId = messageId,
                CarteId = carte.NumeroCarte
            };

            _context.CartEvenements.Add(cartEvenement);
            await _context.SaveChangesAsync();
        }

        

    }
}
