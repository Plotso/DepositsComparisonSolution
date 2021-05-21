namespace DepositsComparisonDomainLogicAPI.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDbEntityService
    {
        IEnumerable<T> GetAll<T>();
        
        T GetById<T>(int id);
        
        Task DeleteAsync(int id);
    }
}