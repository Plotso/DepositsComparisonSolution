namespace DepositsComparisonDomainLogicAPI.Services.Interfaces
{
    using DepositsComparisonDomainLogic.Contracts.Models.Deposits;

    public interface IPaymentPlanGenerator
    {
        PaymentPlan GeneratePaymentPlan(decimal amount, int months, DepositInfo deposit);
    }
}