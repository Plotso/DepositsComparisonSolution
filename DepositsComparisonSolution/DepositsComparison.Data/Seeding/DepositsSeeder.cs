namespace DepositsComparison.Data.Seeding
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Models.Domain;

    public class DepositsSeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IEnumerable<Deposit> deposits)
        {
            if (dbContext.Deposits.Any())
            {
                return;
            }

            foreach (var deposit in deposits)
            {
                var bank = dbContext.Banks.FirstOrDefault(b => b.Name == deposit.Bank.Name);
                deposit.BankId = bank.Id;

                await dbContext.Deposits.AddAsync(deposit);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}