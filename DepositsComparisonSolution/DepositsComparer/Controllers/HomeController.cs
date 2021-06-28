using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DepositsComparer.Models;
using DepositsComparer.Services;
using DepositsComparisonDomainLogic.Contracts.Models.Deposits;

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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
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