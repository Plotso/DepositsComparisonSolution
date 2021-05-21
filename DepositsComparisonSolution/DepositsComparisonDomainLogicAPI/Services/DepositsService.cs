namespace DepositsComparisonDomainLogicAPI.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using DepositsComparison.Data.Models.Domain;
    using DepositsComparison.Data.Repositories;
    using Interfaces;
    using Models;

    public class DepositsService : IDepositsService
    {
        private readonly IDeletableEntityRepository<Deposit> _depositsRepository;
        private readonly IMapper _mapper;

        public DepositsService(IDeletableEntityRepository<Deposit> depositsRepository, IMapper mapper)
        {
            _depositsRepository = depositsRepository;
            _mapper = mapper;
        }
        
        public IEnumerable<T> GetAll<T>()
        {
            var allEntities = _depositsRepository.All();
            return allEntities.Select(e => _mapper.Map<T>(e));
        }

        public T GetById<T>(int id)
        {
            var deposit = _depositsRepository.All().FirstOrDefault(c => c.Id == id);
            return _mapper.Map<T>(deposit);
        }

        public async Task DeleteAsync(int id)
        {
            var deposit = _depositsRepository.All().FirstOrDefault(c => c.Id == id);
            if (deposit != null)
            {
                _depositsRepository.Delete(deposit);
                await _depositsRepository.SaveChangesAsync();
            }
        }

        public Task CreateAsync(DepositCreateInputModel inputModel)
        {
            var deposit = _mapper.Map<Deposit>(inputModel); //ToDo: Ensure this mapping is created
            throw new System.NotImplementedException();
        }
    }
}