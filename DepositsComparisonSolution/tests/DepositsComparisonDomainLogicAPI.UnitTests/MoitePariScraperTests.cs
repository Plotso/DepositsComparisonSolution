namespace DepositsComparisonDomainLogicAPI.UnitTests
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using NUnit.Framework;
    using Services.DataCollecting;

    [TestFixture]
    public class MoitePariScraperTests
    {
        private IWebScraper _scraper;

        [OneTimeSetUp]
        public void Setup()
        {
            _scraper = new MoitePariScraper();
        }

        [Test]
        public async Task GetAllBankProductsAsync_OnACall_ShouldHaveValidResponse()
        {
            var result = await _scraper.GetAllBankProductsAsync();

            result.Should().NotBeNull();
            result.BankProducts.Should().NotBeEmpty();
            result.SourceURL.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task GetDepositsAsync_OnACall_ShouldHaveValidResponse()
        {
            var result = await _scraper.GetDepositsAsync();

            result.Should().NotBeNull();
            result.Deposits.Should().NotBeEmpty();
            result.SourceURL.Should().NotBeNullOrEmpty();
        }
    }
}