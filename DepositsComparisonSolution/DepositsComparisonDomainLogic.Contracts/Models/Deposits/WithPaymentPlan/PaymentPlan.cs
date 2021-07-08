namespace DepositsComparisonDomainLogic.Contracts.Models.Deposits
{
    using System.Collections.Generic;

    public class PaymentPlan
    {
        public decimal DepositAmount { get; set; }
        
        public int Months { get; set; }
        
        public decimal EffectiveAnnualInterest { get; set; }
        
        public decimal InterestTotalSum { get; set; }
        
        /// <summary>
        /// InterestTotalSum minus the tax percentage
        /// </summary>
        public decimal InterestTotalTax { get; set; }
        
        public decimal GrossPaymentAmount { get; set; }
        
        public decimal NetPaymentAmount { get; set; }
        
        public IEnumerable<PaymentPlanEntry> Entries { get; set; }
    }
}