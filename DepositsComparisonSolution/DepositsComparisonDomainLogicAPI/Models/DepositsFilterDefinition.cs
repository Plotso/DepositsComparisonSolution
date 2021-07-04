namespace DepositsComparisonDomainLogicAPI.Models
{
    using DepositsComparison.Data.Public;

    public class DepositsFilterDefinition
    {
        public decimal Amount { get; set; }
        
        public Currency Currency { get; set; }
        
        public int PeriodInMonths { get; set; }
        
        public InterestType InterestType { get; set; }
    }
}