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
            CreateMap<DepositInfo, DepositCreateInputModel>().ReverseMap();
            CreateMap<DepositCreateInputModel, Deposit>();
            
            CreateMap<Bank, BankInfo>().ReverseMap();
            CreateMap<BankProduct, BankProductInfo>().ReverseMap();
            CreateMap<Interest, InterestInfo>().ReverseMap();
            CreateMap<Deposit, DepositInfo>().ReverseMap();
        }
    }
}