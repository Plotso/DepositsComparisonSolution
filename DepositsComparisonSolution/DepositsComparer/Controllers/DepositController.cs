using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DepositsComparer.Models;
using DepositsComparer.Services;
using DepositsComparisonDomainLogic.Contracts.Models.Deposits;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DepositsComparer.Controllers
{
    public class DepositController : Controller
    {
        private readonly ILogger<DepositController> _logger;
        private readonly IDepositsComparisonAPIConsumer _apiConsumer;

        public DepositController(ILogger<DepositController> logger
                                 ,IDepositsComparisonAPIConsumer apiConsumer
            )
        {
            _logger = logger;
            _apiConsumer = apiConsumer;
        }


        public async Task<IActionResult> All()
        {
            var getAllDepositsResponse = await _apiConsumer.GetAllDeposits();
            return View(getAllDepositsResponse);
        }

        public IActionResult GetAllBankProduct()
        {
            Task<IEnumerable<BankInfo>> getAllDepositsResponse = (Task<IEnumerable<BankInfo>>)_apiConsumer.GetAllBankProductsAsync().Result.BankProducts;
            return View(getAllDepositsResponse);
        }

    }
}
