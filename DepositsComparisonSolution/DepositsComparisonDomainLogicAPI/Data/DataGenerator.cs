namespace DepositsComparisonDomainLogicAPI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using DepositsComparison.Data;
    using DepositsComparison.Data.Models.Domain;
    using DepositsComparison.Data.Seeding;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Services.DataCollecting;

    public class DataGenerator
    {
        public async Task GenerateDataAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(DataGenerator));
            var scrapers = serviceProvider.GetServices<IWebScraper>();
            var mapper = serviceProvider.GetService<IMapper>();

            var bankProductsDbSeeder = new BankProductsSeeder();
            var banksDbSeeder = new BanksSeeder();
            var depositsDbSeeder = new DepositsSeeder();
            var interestsDbSeeder = new InterestsSeeder();

            foreach (var scraper in scrapers)
            {
                var getBankProductsResult = await scraper.GetAllBankProductsAsync();
                
                logger.LogInformation($"Scraper {scraper.GetType().Name} generated bank products data from {getBankProductsResult.SourceURL}");
                await bankProductsDbSeeder.SeedAsync(dbContext,
                    getBankProductsResult.BankProducts.Select(p => mapper.Map<BankProduct>(p)));
                
                var getDepositsResult = await scraper.GetDepositsAsync();
                logger.LogInformation($"Scraper {scraper.GetType().Name} generated deposits data from {getDepositsResult.SourceURL}");

                var banks = getDepositsResult.Deposits.Select(d => d.Bank).Distinct();
                await banksDbSeeder.SeedAsync(dbContext, banks.Select(b => mapper.Map<Bank>(b)));

                await depositsDbSeeder.SeedAsync(dbContext,
                    getDepositsResult.Deposits.Select(d => mapper.Map<Deposit>(d)));

                var interests = new List<Interest>();
                foreach (var depositInfo in getDepositsResult.Deposits)
                {
                    var deposit = mapper.Map<Deposit>(depositInfo);
                    foreach (var interestOption in depositInfo.InterestOptions)
                    {
                        var interest = mapper.Map<Interest>(interestOption);
                        interest.Deposit = deposit;
                        interests.Add(interest);
                    }
                }

                await interestsDbSeeder.SeedAsync(dbContext, interests);
            }
        }
    }
}