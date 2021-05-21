namespace DepositsComparisonDomainLogic.Contracts
{
    using System.Collections.Generic;
    using Models;

    public class GetAllBankProductsResponse
    {
        public IEnumerable<BankProductInfo> BankProducts { get; set; }
    }
}