namespace DepositsComparison.Data.Models.Domain
{
    using System.ComponentModel.DataAnnotations;
    using Asbtract;
    using Public;

    public class Interest : BaseDeletableModel<string>
    {
        public int Months { get; set; }

        [Range(0.00, 100.00)]
        public decimal Percentage { get; set; }

        public InterestType Type { get; set; }
        
        [Required]
        public string DepositId { get; set; }

        public virtual Deposit Deposit { get; set; }
    }
}