namespace DepositsComparer.Services
{
    using System.Threading.Tasks;
    using DepositsComparisonDomainLogic.Contracts;

    public interface IDepositsComparisonAPIConsumer
    {
        Task<GetAllBankProductsResponse> GetAllBankProductsAsync();

        Task<GetAllDepositsResponsе> GetAllDeposits();
    }
}