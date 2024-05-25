using API.Data;
using API.Models;

namespace API.Services.HangFireServices.CreditPaymentService
{
    public class CreditPaymentService(DataContext dbContext): ICreditPaymentService
    {
        private readonly DataContext _dbContext = dbContext;

        public async Task ProcessCreditPaymentsAsync()
        {
            var creditsToUpdate = _dbContext.Credits
                .Where(c => c.CreditMensuelle > 0 &&
                (c.LastVerificationDate.Month != DateTime.Now.Month) &&
                (c.JourPaiement == DateTime.Now.Day))
                .ToList();

            foreach (var credit in creditsToUpdate)
            {
                var soldeCompte = credit.Compte.SoldeCompte;
                var canalPrefere = credit.Compte.Client.CanalPrefere;

                if (credit.CreditMensuelle > soldeCompte)
                {
                    _dbContext.CreditEvenements.Add(new CreditEvenement
                    {
                        Canal = canalPrefere,
                        HeureEnvoi = DateTime.Now.AddHours(1),
                        TemplateId = 6,
                        CreditId = credit.Id
                    });
                }
                else
                {
                    credit.RestCredit = Math.Max(credit.RestCredit - credit.CreditMensuelle, 0);
                    credit.LastVerificationDate = DateTime.Now;

                    _dbContext.CreditEvenements.Add(new CreditEvenement
                    {
                        Canal = canalPrefere,
                        HeureEnvoi = DateTime.Now.AddHours(1),
                        TemplateId = 5,
                        CreditId = credit.Id
                    });

                    if (credit.RestCredit == 0)
                    {
                        _dbContext.CreditEvenements.Add(new CreditEvenement
                        {
                            Canal = canalPrefere,
                            HeureEnvoi = DateTime.Now.AddHours(1),
                            TemplateId = 7,
                            CreditId = credit.Id
                        });
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }

    }
}
