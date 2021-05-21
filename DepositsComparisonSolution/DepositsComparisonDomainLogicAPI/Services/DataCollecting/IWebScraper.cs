namespace DepositsComparisonDomainLogicAPI.Services.DataCollecting
{
    using System.Threading.Tasks;
    using Models.DataCollecting;

    public interface IWebScraper
    {
        Task<GetBankProductsResult> GetAllBankProductsAsync();

        Task<GetDepositsResult> GetDepositsAsync();
    }
}