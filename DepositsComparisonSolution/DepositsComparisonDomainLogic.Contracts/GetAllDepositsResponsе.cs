namespace DepositsComparisonDomainLogic.Contracts
{
    using System.Collections.Generic;
    using Models.Deposits;

    public class GetAllDepositsResponsе
    {
        public IEnumerable<DepositInfo> Deposits { get; set; }
    }
}