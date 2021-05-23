namespace DepositsComparisonDomainLogic.Contracts.Models.Deposits
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DepositsComparison.Data.Public;

    public class DepositInfo
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public BankInfo Bank { get; set; }

        public decimal MinAmount { get; set; }

        public decimal? MaxAmount { get; set; }
        
        public string InterestDetails { get; set; }

        public IEnumerable<InterestInfo> InterestOptions { get; set; } = new List<InterestInfo>();
        
        public string InterestPaymentInfo { get; set; }

        public Currency Currency { get; set; }
    }
}