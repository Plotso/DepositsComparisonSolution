using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DepositsComparer.Models;
using DepositsComparer.Services;
using DepositsComparisonDomainLogic.Contracts.Models;
using DepositsComparisonDomainLogic.Contracts.Models.Deposits;
using System.Threading.Tasks;

namespace DepositsComparer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDepositsComparisonAPIConsumer _apiConsumer;

        public HomeController(ILogger<HomeController> logger
                             ,IDepositsComparisonAPIConsumer apiConsumer
            )
        {
            _logger = logger;
            _apiConsumer = apiConsumer;
        }

        public async Task<IActionResult> Index()
        {
            var depositResponce = await _apiConsumer.GetAllDeposits();
            //var bankResponce = await _apiConsumer.GetAllBankProductsAsync();
            return View(depositResponce);
        }

        //public async Task<IActionResult> Comparer(string name)
        //{
        //    // If there are more bank products - fix it.
        //    var getAllDepositResponse = new List<DepositInfo>() { };
        //    foreach (var product in _apiConsumer.GetAllDeposits().Result.Deposits)
        //    {
        //        if (product.Name.ToLower() == name.ToLower())
        //        {
        //            getAllDepositResponse.Add(product);
        //        }
        //    }
        //    return View(getAllDepositResponse);
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult PageNotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}