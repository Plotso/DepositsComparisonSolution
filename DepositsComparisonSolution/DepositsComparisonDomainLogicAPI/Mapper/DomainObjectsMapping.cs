namespace DepositsComparisonDomainLogicAPI.Mapper
{
    using AutoMapper;
    using DepositsComparison.Data.Models.Domain;
    using DepositsComparisonDomainLogic.Contracts.Models;
    using DepositsComparisonDomainLogic.Contracts.Models.Deposits;
    using Models;

    public class DomainObjectsMapping : Profile
    {
        public DomainObjectsMapping()
        {
            CreateMap<DepositInfo, DepositCreateInputModel>();
            
            CreateMap<Bank, BankInfo>().ReverseMap();
            CreateMap<BankProduct, BankProductInfo>().ReverseMap();
            CreateMap<Deposit, DepositInfo>().ReverseMap();
            CreateMap<Bank, BankInfo>().ReverseMap();
            CreateMap<Interest, InterestInfo>().ReverseMap();
        }
    }
}