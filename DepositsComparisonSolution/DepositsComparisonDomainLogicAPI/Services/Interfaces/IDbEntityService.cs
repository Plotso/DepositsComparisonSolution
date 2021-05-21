namespace DepositsComparisonDomainLogicAPI.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDbEntityService
    {
        IEnumerable<T> GetAll<T>();
        
        T GetById<T>(string id);
        
        Task DeleteAsync(string id);
    }
}