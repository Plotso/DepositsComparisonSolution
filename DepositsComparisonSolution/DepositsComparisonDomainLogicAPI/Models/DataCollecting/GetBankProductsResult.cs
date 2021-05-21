namespace DepositsComparisonDomainLogicAPI.Models.DataCollecting
{
    using System.Collections.Generic;
    using DepositsComparisonDomainLogic.Contracts.Models;

    public class GetBankProductsResult
    {
        public string SourceURL { get; set; }

        public IList<BankProductInfo> BankProducts { get; set; }
    }
}