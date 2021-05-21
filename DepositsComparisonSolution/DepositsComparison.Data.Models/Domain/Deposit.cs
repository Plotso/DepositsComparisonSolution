namespace DepositsComparison.Data.Models.Domain
{
    using System.Collections.Generic;
    using Asbtract;
    using Public;

    public class Deposit : BaseDeletableModel<string>
    {
        public Deposit()
        {
            InterestOptions = new HashSet<Interest>();
        }
        
        public string Name { get; set; }
        
        public string BankId { get; set; }
        
        public virtual Bank Bank { get; set; }

        public decimal MinAmount { get; set; }

        public decimal MaxAmount { get; set; }
        
        public string InterestDetails { get; set; }
        
        public virtual ICollection<Interest> InterestOptions { get; set; }
        
        public string InterestPaymentInfo { get; set; }

        public Currency Currency { get; set; }
    }
}