namespace DepositsComparisonDomainLogicAPI.Models
{
    using DepositsComparison.Data.Models.Domain;
    using DepositsComparison.Data.Public;

    public class DepositCreateInputModel
    {
        public string Name { get; set; }
        
        public string BankId { get; set; }

        public decimal MinAmount { get; set; }

        public decimal MaxAmount { get; set; }
        
        public string InterestDetails { get; set; }
        
        public string InterestPaymentInfo { get; set; }

        public Currency Currency { get; set; }
    }
}