namespace DepositsComparisonDomainLogic.Contracts
{
    using System.Collections.Generic;
    using Models.Deposits;

    public class GetAllBanksResponse
    {
        public IEnumerable<BankInfo> Banks { get; set; }
    }
}