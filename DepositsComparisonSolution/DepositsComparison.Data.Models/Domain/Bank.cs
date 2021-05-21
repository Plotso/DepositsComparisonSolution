namespace DepositsComparison.Data.Models.Domain
{
    using System.Collections.Generic;
    using Asbtract;

    public class Bank : BaseDeletableModel<string>
    {
        public Bank()
        {
            Deposits = new HashSet<Deposit>();
        }
        
        public string Name { get; set; }
        
        public virtual ICollection<Deposit> Deposits { get; set; } 
    }
}