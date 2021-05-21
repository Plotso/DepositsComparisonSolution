namespace DepositsComparisonDomainLogicAPI.Models
{
    using DepositsComparison.Data.Public;
    using DepositsComparisonDomainLogic.Contracts.Models.Deposits;

    //ToDo: Map to DepositInfo object
    public class DepositCreateInputModel
    {
        
        public string Name { get; set; }
        
        public int BankId { get; set; }

        public decimal MinAmount { get; set; }

        public decimal MaxAmount { get; set; }
        
        public string InterestDetails { get; set; }
        
        // TOdo: Decide about deposits
        //public InterestInfo[] InterestOptions { get; set; }
        
        public string InterestPaymentInfo { get; set; }

        public Currency Currency { get; set; }
    }
}