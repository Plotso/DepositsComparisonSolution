namespace DepositsComparisonDomainLogicAPI.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using DepositsComparison.Data.Models.Domain;
    using DepositsComparison.Data.Public;
    using DepositsComparison.Data.Repositories;
    using Interfaces;

    public class InterestService : IInterestService
    {
        private readonly IDeletableEntityRepository<Interest> _interestsRepository;
        private readonly IMapper _mapper;

        public InterestService(IDeletableEntityRepository<Interest> interestsRepository, IMapper mapper)
        {
            _interestsRepository = interestsRepository;
            _mapper = mapper;
        }
        
        public IEnumerable<T> GetAll<T>()
        {
            var allEntities = _interestsRepository.All();
            return allEntities.Select(e => _mapper.Map<T>(e));
        }

        public T GetById<T>(string id)
        {
            var interest = _interestsRepository.All().FirstOrDefault(c => c.Id == id);
            return _mapper.Map<T>(interest);
        }

        public async Task DeleteAsync(string id)
        {
            var interest = _interestsRepository.All().FirstOrDefault(c => c.Id == id);
            if (interest != null)
            {
                _interestsRepository.Delete(interest);
                await _interestsRepository.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(int months, decimal percentage, InterestType type, string depositId)
        {
            if (string.IsNullOrEmpty(depositId))
            {
                throw new ArgumentException("DepositId is mandatory for the creation of an Interest");
            }
            
            var interest = new Interest
            {
                Months = months,
                Percentage = percentage,
                Type = type,
                DepositId = depositId
            };
            
            await _interestsRepository.AddAsync(interest);
            await _interestsRepository.SaveChangesAsync();
        }
    }
}