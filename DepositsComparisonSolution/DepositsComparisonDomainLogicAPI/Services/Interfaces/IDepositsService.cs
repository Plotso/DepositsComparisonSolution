namespace DepositsComparisonDomainLogicAPI.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IDepositsService : IDbEntityService
    {
        IEnumerable<T> GetFiltered<T>(DepositsFilterDefinition filterDefinition);
        
        Task<string> CreateAsync(DepositCreateInputModel inputModel);
    }
}