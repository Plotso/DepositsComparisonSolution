namespace DepositsComparisonDomainLogic.Contracts
{
    using System.Collections.Generic;
    using Models;

    public class GetAllBankProductsResponse
    {
        public IEnumerable<BankProduct> BankProducts { get; set; }
    }
}