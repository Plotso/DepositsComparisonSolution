namespace DepositsComparisonDomainLogic.Contracts
{
    using System.ComponentModel.DataAnnotations;
    using DepositsComparison.Data.Public;

    public class GetFilteredDepositsRequest
    {
        public decimal Amount { get; set; }
        
        [Required]
        public Currency Currency { get; set; }
        
        public int PeriodInMonths { get; set; }
        
        [Required]
        public InterestType InterestType { get; set; }
    }
}