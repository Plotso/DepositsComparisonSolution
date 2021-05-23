namespace DepositsComparison.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
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
                var bank = await dbContext.Banks.FirstOrDefaultAsync(b => b.Name == deposit.Bank.Name);

                if (bank == null)
                {
                    throw new InvalidOperationException(
                        $"Cannot create interest because there is no bank with name {deposit.Bank.Name} inside database");
                }
                deposit.BankId = bank.Id;
                deposit.Bank = bank;
                deposit.InterestOptions = new List<Interest>();

                deposit.Id = Guid.NewGuid().ToString();
                await dbContext.Deposits.AddAsync(deposit);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}