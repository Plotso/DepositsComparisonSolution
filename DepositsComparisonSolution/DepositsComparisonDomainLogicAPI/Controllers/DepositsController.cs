namespace DepositsComparisonDomainLogicAPI.Controllers
{
    using DepositsComparisonDomainLogic.Contracts;
    using DepositsComparisonDomainLogic.Contracts.Models.Deposits;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;
    using Services.Interfaces;

    [ApiController]
    [Route("[controller]")]
    public class DepositsController : ControllerBase
    {
        private readonly ILogger<DepositsController> _logger;
        private readonly IDepositsService _depositsService;

        public DepositsController(ILogger<DepositsController> logger, IDepositsService depositsService)
        {
            _logger = logger;
            _depositsService = depositsService;
        }

        [HttpGet(nameof(GetAllDeposits))]
        public GetAllDepositsResponsе GetAllDeposits()
        {
            var deposits = _depositsService.GetAll<DepositInfo>();
            
            return new GetAllDepositsResponsе
            {
                Deposits = deposits
            };
        }

        [HttpGet(nameof(GetFilteredDeposits))]
        public GetFilteredDepositsResponse GetFilteredDeposits(GetFilteredDepositsRequest request)
        {
            var filterDefinition = new DepositsFilterDefinition
            {
                Amount = request.Amount,
                Currency = request.Currency,
                InterestType = request.InterestType,
                PeriodInMonths = request.PeriodInMonths
            };
            
            var deposits = _depositsService.GetFiltered<DepositInfo>(filterDefinition);
            
            return new GetFilteredDepositsResponse
            {
                Deposits = deposits
            };
        }
    }
}