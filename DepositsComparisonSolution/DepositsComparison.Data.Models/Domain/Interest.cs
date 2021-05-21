namespace DepositsComparison.Data.Models.Domain
{
    using Asbtract;
    using Public;

    public class Interest : BaseDeletableModel<int>
    {
        public int Months { get; set; }
        
        public decimal Percentage { get; set; }
        
        public InterestType Type { get; set; }
    }
}