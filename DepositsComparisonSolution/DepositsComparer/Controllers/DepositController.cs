namespace DepositsComparer.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using DepositsComparer.Services;
    using DepositsComparisonDomainLogic.Contracts.Models.Deposits;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using DepositsComparisonDomainLogic.Contracts;

    public class DepositController : Controller
    {
        private readonly ILogger<DepositController> _logger;
        private readonly IDepositsComparisonAPIConsumer _apiConsumer;

        private readonly IFilteredDepositCollection _filteredDepositCollection;

        public DepositController(ILogger<DepositController> logger
                                 ,IDepositsComparisonAPIConsumer apiConsumer
                                 ,IFilteredDepositCollection filteredDeposit
            )
        {
            _logger = logger;
            _apiConsumer = apiConsumer;
            _filteredDepositCollection = filteredDeposit;
        }


        public async Task<IActionResult> All()
        {
            var getAllDepositsResponse = await _apiConsumer
                .GetAllDepositsAsync();
            return View(getAllDepositsResponse.Deposits
                .OrderBy(x => x.Bank.Name).ThenBy(m => m.MinAmount));
        }

        public IActionResult Comparer(string id)
        {
            if (id == "Депозити и сметки")
            {
                return View();
            }
            return Redirect("Home/PageNotFound");
        }

        public async Task<IActionResult> ComparerResult(GetFilteredDepositsRequest filter)
        {
            var filteredDeposits = await _apiConsumer.GetFilteredDepositsAsync(filter.Amount, filter.Currency, filter.InterestType, filter.PeriodInMonths);
            _filteredDepositCollection.SetDeposits(filteredDeposits.Deposits);
            var deposit = filteredDeposits.Deposits.Select(x => x.Deposit);
            var paymentPlans = filteredDeposits.Deposits.Select(x => x.PaymentPlan);
            return View((IEnumerable<DepositInfoWithPaymentPlan>)_filteredDepositCollection.GetAll());
        }

        public async Task<IActionResult> Details(string id)
        {
            var getCurrDepositResponse = await _apiConsumer
                .GetAllDepositsAsync();
            return View(getCurrDepositResponse.Deposits
                .FirstOrDefault(x => x.Name == id));
        }

        public IActionResult DetailsWithPlan(string id)
        {
            var depositsInfoPaymentPlan = _filteredDepositCollection.GetByDepositName(id);
            if (depositsInfoPaymentPlan == null)
            {
                return Redirect("Home/PageNotFound");
            }
            return View(depositsInfoPaymentPlan);
        }

        public IActionResult Currency(string id)
        {
            decimal percentage = 1m;
            if (id == "EUR")
            {
                percentage = 0.51m;
            }
            else if (id == "USD")
            {
                percentage = 0.61m;
            }
            var getAllDepositResponse = _apiConsumer
                .GetAllDepositsAsync().Result.Deposits
                .Select(x =>  new DepositInfo
                {
                    Name = x.Name,
                    Bank = x.Bank,
                    MinAmount = (x.MinAmount * percentage),
                    MaxAmount = x.MaxAmount,
                    InterestDetails = x.InterestDetails,
                    InterestPaymentInfo = x.InterestPaymentInfo,
                    Currency = x.Currency,
                    InterestOptions = x.InterestOptions
                })
                .OrderBy(c => c.Name);
            return View("All", getAllDepositResponse);
        }
        public async Task<IActionResult> AllOrderByType()
        {
            var getAllDepositsResponse = await _apiConsumer
                .GetAllDepositsAsync();
            return View("All", getAllDepositsResponse.Deposits
                .OrderBy(x => x.Name));
        }

        public async Task<IActionResult> AllOrderByPaymentType()
        {
            var getAllDepositsResponse = await _apiConsumer
                .GetAllDepositsAsync();
            return View("All", getAllDepositsResponse.Deposits
                .OrderBy(x => x.InterestPaymentInfo));
        }

        public IActionResult PaymentPlan(string id)
        {
            foreach (var item in _filteredDepositCollection.GetAll())
            {
                if (item.Deposit.Name == id)
                {
                    return View(item.PaymentPlan);
                }
            }
            return Redirect("Home/PageNotFound");
        }
    }
}
