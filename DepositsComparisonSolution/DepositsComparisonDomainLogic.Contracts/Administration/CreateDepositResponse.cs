namespace DepositsComparisonDomainLogic.Contracts
{
    public class CreateDepositResponse
    {
        public bool IsSuccess { get; set; }
        
        public string ErrorMessage { get; set; }
    }
}