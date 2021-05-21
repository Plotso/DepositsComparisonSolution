namespace DepositsComparisonDomainLogicAPI.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using DepositsComparison.Data.Models.Domain;
    using DepositsComparison.Data.Repositories;
    using Interfaces;

    public class BankService : IBankService
    {
        private readonly IDeletableEntityRepository<Bank> _banksRepository;
        private readonly IMapper _mapper;

        public BankService(IDeletableEntityRepository<Bank> banksRepository, IMapper mapper)
        {
            _banksRepository = banksRepository;
            _mapper = mapper;
        }
        
        public IEnumerable<T> GetAll<T>()
        {
            var allEntities = _banksRepository.All();
            return allEntities.Select(e => _mapper.Map<T>(e));
        }

        public T GetById<T>(int id)
        {
            var bank = _banksRepository.All().FirstOrDefault(c => c.Id == id);
            return _mapper.Map<T>(bank);
        }

        public async Task DeleteAsync(int id)
        {
            var bank = _banksRepository.All().FirstOrDefault(c => c.Id == id);
            if (bank != null)
            {
                _banksRepository.Delete(bank);
                await _banksRepository.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(string bankName)
        {
            var bank = new Bank {Name = bankName};
            await _banksRepository.AddAsync(bank);
            await _banksRepository.SaveChangesAsync();
        }
    }
}