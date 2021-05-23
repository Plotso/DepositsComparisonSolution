namespace DepositsComparisonDomainLogic.Contracts.Models.Deposits
{
    using System.ComponentModel.DataAnnotations;

    public class BankInfo
    {
        [Required]
        public string Name { get; set; }
    }
}