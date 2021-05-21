namespace DepositsComparison.Data.Models.Domain
{
    using Asbtract;
    using Public;

    public class Interest : BaseDeletableModel<string>
    {
        public int Months { get; set; }

        public decimal Percentage { get; set; }

        public InterestType Type { get; set; }
        
        public string DepositId { get; set; }

        public virtual Deposit Deposit { get; set; }
    }
}