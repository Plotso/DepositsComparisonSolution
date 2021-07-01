namespace DepositsComparisonDomainLogicAPI.Controllers
{
    using DepositsComparisonDomainLogic.Contracts;
    using DepositsComparisonDomainLogic.Contracts.Models.Deposits;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services.Interfaces;

    [ApiController]
    [Route("[controller]")]
    public class BanksController : ControllerBase
    {
        private readonly ILogger<BanksController> _logger;
        private readonly IBankService _bankService;

        public BanksController(ILogger<BanksController> logger, IBankService bankService)
        {
            _logger = logger;
            _bankService = bankService;
        }

        [HttpGet(nameof(GetAllBanks))]
        public GetAllBanksResponse GetAllBanks()
        {
            var banks = _bankService.GetAll<BankInfo>();

            return new GetAllBanksResponse
            {
                Banks = banks
            };
        }
    }
}