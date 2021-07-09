namespace DepositsComparisonDomainLogic.Contracts.Models.Deposits
{
    public class PaymentPlanEntry
    {
        public int Month { get; set; }

        public decimal InterestPercentage { get; set; }

        public decimal InterestAmount { get; set; }
        
        public decimal InterestTax { get; set; }
        
        public decimal PaymentAmount { get; set; }
        
        public decimal DepositAmount { get; set; }
    }
}