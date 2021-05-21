namespace DepositsComparisonDomainLogicAPI.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBankService : IDbEntityService
    {
        Task CreateAsync(string bankName);

        string GetBankIdByName(string bankName);
    }
}