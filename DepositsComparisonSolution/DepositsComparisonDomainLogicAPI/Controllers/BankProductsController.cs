namespace DepositsComparisonDomainLogicAPI.Controllers
{
    using DepositsComparisonDomainLogic.Contracts;
    using DepositsComparisonDomainLogic.Contracts.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services.Interfaces;

    [ApiController]
    [Route("[controller]")]
    public class BankProductsController : ControllerBase
    {
        private readonly ILogger<BankProductsController> _logger;
        private readonly IBankProductsService _bankProductsService;

        public BankProductsController(ILogger<BankProductsController> logger, IBankProductsService bankProductsService)
        {
            _logger = logger;
            _bankProductsService = bankProductsService;
        }

        [HttpGet]
        public GetAllBankProductsResponse GetBankProducts()
        {
            var bankProducts = _bankProductsService.GetAll<BankProductInfo>();
            
            return new GetAllBankProductsResponse
            {
                BankProducts = bankProducts
            };
        }
    }
}