namespace DepositsComparison.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
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
                var deposit = await dbContext.Deposits.FirstOrDefaultAsync(d =>
                    d.Name == interest.Deposit.Name &&
                    d.Bank.Name == interest.Deposit.Bank.Name &&
                    d.Currency == interest.Deposit.Currency);

                if (deposit == null)
                {
                    throw new InvalidOperationException(
                        $"Cannot create interest because there is no deposit with name {interest.Deposit.Name} inside database");
                }
                
                interest.DepositId = deposit.Id;
                interest.Deposit = deposit;
                interest.Id = Guid.NewGuid().ToString();
                
                await dbContext.Interests.AddAsync(interest);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}