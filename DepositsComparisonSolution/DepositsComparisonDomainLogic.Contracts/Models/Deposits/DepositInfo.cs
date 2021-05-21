namespace DepositsComparisonDomainLogic.Contracts.Models.Deposits
{
    using DepositsComparison.Data.Public;

    public class DepositInfo
    {
        public string Name { get; set; }
        
        public BankInfo Bank { get; set; }

        public decimal MinAmount { get; set; }

        public decimal? MaxAmount { get; set; }
        
        public string InterestDetails { get; set; }
        
        public InterestInfo[] InterestOptions { get; set; }
        
        public string InterestPaymentInfo { get; set; }

        public Currency Currency { get; set; }
    }
}