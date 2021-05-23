namespace DepositsComparison.Data.Seeding
{
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

            await dbContext.BankProducts.AddRangeAsync(bankProducts);
            await dbContext.SaveChangesAsync();
        }
    }
}