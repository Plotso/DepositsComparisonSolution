namespace DepositsComparisonDomainLogicAPI.Models.DataCollecting
{
    using System.Collections.Generic;
    using DepositsComparisonDomainLogic.Contracts.Models.Deposits;

    public class GetDepositsResult
    {
        public GetDepositsResult()
        {
            Deposits = new List<DepositInfo>();
        }
        
        public string SourceURL { get; set; }
        
        public IList<DepositInfo> Deposits { get; set; }
    }
}