namespace DepositsComparisonDomainLogic.Contracts
{
    using Models.Deposits;

    public class CreateDepositRequest
    {
        public DepositInfo Deposit { get; set; }
    }
}