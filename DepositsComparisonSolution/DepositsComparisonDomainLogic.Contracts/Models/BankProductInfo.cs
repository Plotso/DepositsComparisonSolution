namespace DepositsComparisonDomainLogic.Contracts.Models
{
    using DepositsComparison.Data.Public;

    public class BankProductInfo
    {
        public string Name { get; set; }

        public BankProductType Type { get; set; }
    }
}