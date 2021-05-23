namespace DepositsComparison.Data.Models.Domain
{
    using System.ComponentModel.DataAnnotations;
    using Asbtract;
    using Public;

    public class BankProduct : BaseDeletableModel<string>
    {
        [Required]
        public string Name { get; set; }

        public BankProductType Type { get; set; }
    }
}