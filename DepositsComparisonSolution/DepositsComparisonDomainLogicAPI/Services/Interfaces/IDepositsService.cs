namespace DepositsComparisonDomainLogicAPI.Services.Interfaces
{
    using System.Threading.Tasks;
    using Models;

    public interface IDepositsService : IDbEntityService
    {
        Task CreateAsync(DepositCreateInputModel inputModel);
    }
}