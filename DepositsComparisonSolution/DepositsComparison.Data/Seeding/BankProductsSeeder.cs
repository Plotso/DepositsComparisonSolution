namespace DepositsComparison.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Models.Domain;

    public class BankProductsSeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IEnumerable<BankProduct> bankProducts)
        {
            if (dbContext.BankProducts.Any())
            {
                return;
            }
            
            foreach (var bankProduct in bankProducts)
            {
                bankProduct.Id = Guid.NewGuid().ToString();
                await dbContext.BankProducts.AddAsync(bankProduct);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}