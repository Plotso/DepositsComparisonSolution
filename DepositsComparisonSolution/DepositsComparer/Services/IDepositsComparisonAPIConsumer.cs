namespace DepositsComparer.Services
{
    using System.Threading.Tasks;
    using DepositsComparison.Data.Public;
    using DepositsComparisonDomainLogic.Contracts;

    public interface IDepositsComparisonAPIConsumer
    {
        Task<GetAllBankProductsResponse> GetAllBankProductsAsync();

        Task<GetAllDepositsResponsе> GetAllDepositsAsync();

        Task<GetFilteredDepositsResponse> GetFilteredDepositsAsync(
            decimal amount, 
            Currency currency,
            InterestType interestType, 
            int periodInMonths);
    }
}