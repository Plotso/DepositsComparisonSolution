namespace DepositsComparison.Data.Models.Domain
{
    using Asbtract;
    using Public;

    public class Deposit : BaseDeletableModel<int>
    {
        public string Name { get; set; }
        
        public Bank Bank { get; set; }

        public decimal MinAmount { get; set; }

        public decimal MaxAmount { get; set; }
        
        public string InterestDetails { get; set; }
        
        public Interest[] InterestOptions { get; set; }
        
        public string InterestPaymentInfo { get; set; }

        public Currency Currency { get; set; }
    }
}