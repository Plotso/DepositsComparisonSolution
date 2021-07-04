namespace DepositsComparisonDomainLogicAPI.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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
        private readonly IBankService _bankService;

        public DepositsService(IDeletableEntityRepository<Deposit> depositsRepository, IMapper mapper, IBankService bankService)
        {
            _depositsRepository = depositsRepository;
            _mapper = mapper;
            _bankService = bankService;
        }
        
        public IEnumerable<T> GetAll<T>()
        {
            var allEntities = _depositsRepository.All();
            return allEntities.Select(e => _mapper.Map<T>(e));
        }

        public IEnumerable<T> GetFiltered<T>(DepositsFilterDefinition filterDefinition)
        {
            var entities = _depositsRepository.All()
                .Where(d =>
                    d.MinAmount <= filterDefinition.Amount &&
                    (d.MaxAmount == null || d.MaxAmount >= filterDefinition.Amount) &&
                    d.Currency == filterDefinition.Currency &&
                    d.InterestOptions.Any(i => i.Type == filterDefinition.InterestType));

            return entities.Select(e => _mapper.Map<T>(e));
        }

        public T GetById<T>(string id)
        {
            var deposit = _depositsRepository.All().FirstOrDefault(c => c.Id == id);
            return _mapper.Map<T>(deposit);
        }

        public async Task DeleteAsync(string id)
        {
            var deposit = _depositsRepository.All().FirstOrDefault(c => c.Id == id);
            if (deposit != null)
            {
                _depositsRepository.Delete(deposit);
                await _depositsRepository.SaveChangesAsync();
            }
        }

        public async Task<string> CreateAsync(DepositCreateInputModel inputModel)
        {
            if (inputModel.MinAmount < 0.00m)
            {
                throw new InvalidDataException("The minimal amount for the deposit cannot be negative");
            }
            if (inputModel.MaxAmount.HasValue && inputModel.MaxAmount.Value < 0.00m)
            {
                throw new InvalidDataException("The max amount for the deposit cannot be negative");
            }
            
            var deposit = _mapper.Map<Deposit>(inputModel);
            deposit.BankId = inputModel.BankId;
            deposit.Id = Guid.NewGuid().ToString();

            await _depositsRepository.AddAsync(deposit);
            await _depositsRepository.SaveChangesAsync();

            return deposit.Id;
        }
    }
}