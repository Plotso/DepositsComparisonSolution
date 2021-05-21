namespace DepositsComparisonDomainLogicAPI.Services.Interfaces
{
    using System.Threading.Tasks;
    using DepositsComparison.Data.Public;

    public interface IInterestService: IDbEntityService
    {
        Task CreateAsync(int months, decimal percentage, InterestType type, string depositId);
    }
}