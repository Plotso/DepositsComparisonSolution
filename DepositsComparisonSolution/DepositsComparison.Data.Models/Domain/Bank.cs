namespace DepositsComparison.Data.Models.Domain
{
    using System.Collections.Generic;
    using Asbtract;

    public class Bank : BaseDeletableModel<int>
    {
        public string Name { get; set; }
        
        public virtual ICollection<Deposit> Deposits { get; set; } 
    }
}