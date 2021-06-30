using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DepositsComparer.Models;
using DepositsComparer.Services;
using DepositsComparisonDomainLogic.Contracts.Models.Deposits;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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
            var getAllDepositsResponse = _apiConsumer.GetAllDeposits().Result.Deposits;
            return View(getAllDepositsResponse.OrderBy(x => x.Bank.Name).ThenBy(m => m.MinAmount));
        }

        public async Task<IActionResult> AllOrderByType()
        {
            var getAllDepositsResponse = _apiConsumer.GetAllDeposits().Result.Deposits;

            return View("All", getAllDepositsResponse.OrderBy(x => x.Name));
        }


        public async Task<IActionResult> AllOrderByPaymentType()
        {
            var getAllDepositsResponse = _apiConsumer.GetAllDeposits().Result.Deposits;
            return View("All", getAllDepositsResponse.OrderBy(x => x.InterestPaymentInfo));
        }

        public async Task<IActionResult> Comparer()
        {
            var getAllbankproductResponse = _apiConsumer.GetAllBankProductsAsync().Result.BankProducts;
            var getAllDepositResponse = _apiConsumer.GetAllDeposits().Result.Deposits;
            return View(getAllDepositResponse);
        }


        public IActionResult GetAllBankProduct()
        {
            Task<IEnumerable<BankInfo>> getAllDepositsResponse = (Task<IEnumerable<BankInfo>>)_apiConsumer.GetAllBankProductsAsync().Result.BankProducts;
            return View(getAllDepositsResponse);
        }

    }
}
