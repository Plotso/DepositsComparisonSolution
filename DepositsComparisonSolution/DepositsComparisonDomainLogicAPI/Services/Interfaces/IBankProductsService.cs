namespace DepositsComparisonDomainLogicAPI.Services.Interfaces
{
    using System.Threading.Tasks;
    using DepositsComparison.Data.Public;

    public interface IBankProductsService: IDbEntityService
    {
        Task CreateAsync(string name, BankProductType type);
    }
}