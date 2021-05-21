namespace DepositsComparisonDomainLogicAPI.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using DepositsComparison.Data.Models.Domain;
    using DepositsComparison.Data.Public;
    using DepositsComparison.Data.Repositories;
    using Interfaces;

    public class BankProductsService : IBankProductsService
    {
        private readonly IDeletableEntityRepository<BankProduct> _bankProductsRepository;
        private readonly IMapper _mapper;

        public BankProductsService(IDeletableEntityRepository<BankProduct> bankProductsRepository, IMapper mapper)
        {
            _bankProductsRepository = bankProductsRepository;
            _mapper = mapper;
        }
        
        public IEnumerable<T> GetAll<T>()
        {
            var allEntities = _bankProductsRepository.All();
            return allEntities.Select(e => _mapper.Map<T>(e));
        }

        public T GetById<T>(int id)
        {
            var bankProduct = _bankProductsRepository.All().FirstOrDefault(c => c.Id == id);
            return _mapper.Map<T>(bankProduct);
        }

        public async Task DeleteAsync(int id)
        {
            var bankProduct = _bankProductsRepository.All().FirstOrDefault(c => c.Id == id);
            if (bankProduct != null)
            {
                _bankProductsRepository.Delete(bankProduct);
                await _bankProductsRepository.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(string name, BankProductType type)
        {
            
            var bankProduct = new BankProduct
            {
                Name = name,
                Type = type
            };
            
            await _bankProductsRepository.AddAsync(bankProduct);
            await _bankProductsRepository.SaveChangesAsync();
        }
    }
}