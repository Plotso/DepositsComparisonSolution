namespace DepositsComparisonDomainLogic.Contracts
{
    using System.Collections.Generic;
    using Models.Deposits;

    public class GetFilteredDepositsResponse
    {
        public IEnumerable<DepositInfoWithPaymentPlan> Deposits { get; set; }
    }
}