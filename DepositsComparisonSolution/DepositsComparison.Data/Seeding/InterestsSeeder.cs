namespace DepositsComparison.Data.Seeding
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Models.Domain;

    public class InterestsSeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IEnumerable<Interest> interests)
        {
            if (dbContext.Interests.Any())
            {
                return;
            }

            foreach (var interest in interests)
            {
                var deposit = dbContext.Deposits.FirstOrDefault(d =>
                    d.Name == interest.Deposit.Name &&
                    d.Bank.Name == interest.Deposit.Bank.Name &&
                    d.Currency == interest.Deposit.Currency);
                interest.DepositId = deposit.Id;

                await dbContext.Interests.AddAsync(interest);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}