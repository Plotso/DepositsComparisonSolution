namespace DepositsComparison.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Models.Domain;

    public class BanksSeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IEnumerable<Bank> banks)
        {
            if (dbContext.Banks.Any())
            {
                return;
            }

            foreach (var bank in banks)
            {
                if (!dbContext.Banks.Any(b => b.Name == bank.Name))
                {
                    bank.Id = Guid.NewGuid().ToString();
                    await dbContext.Banks.AddAsync(bank);
                }
            }
            await dbContext.SaveChangesAsync();
        }
    }
}