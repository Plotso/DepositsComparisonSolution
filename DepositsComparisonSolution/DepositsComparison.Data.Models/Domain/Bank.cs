namespace DepositsComparison.Data.Models.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Asbtract;

    public class Bank : BaseDeletableModel<string>
    {
        public Bank()
        {
            Deposits = new HashSet<Deposit>();
        }
        
        [Required]
        public string Name { get; set; }
        
        public virtual ICollection<Deposit> Deposits { get; set; } 
    }
}