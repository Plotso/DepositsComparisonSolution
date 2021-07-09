namespace DepositsComparisonDomainLogic.Contracts.Models.Deposits
{
    public class DepositInfoWithPaymentPlan
    {
        public DepositInfo Deposit { get; set; }

        public PaymentPlan PaymentPlan { get; set; }
    }
}