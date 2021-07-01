namespace DepositsComparisonDomainLogic.Contracts
{
    using System.Collections.Generic;
    using Models.Deposits;

    public class GetFilteredDepositsResponse
    {
        public IEnumerable<DepositInfo> Deposits { get; set; }
    }
}