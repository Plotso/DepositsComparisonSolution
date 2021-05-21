namespace DepositsComparison.Data.Models.Domain
{
    using Asbtract;
    using Public;

    public class BankProduct : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public BankProductType Type { get; set; }
    }
}