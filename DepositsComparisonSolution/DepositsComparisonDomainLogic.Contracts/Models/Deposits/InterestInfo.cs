namespace DepositsComparisonDomainLogic.Contracts.Models.Deposits
{
    using System.ComponentModel.DataAnnotations;
    using DepositsComparison.Data.Public;

    public class InterestInfo
    {
        public int Months { get; set; }
        
        
        [Range(0.00, 100.00)]
        public decimal Percentage { get; set; }
        
        public InterestType Type { get; set; }
    }
}