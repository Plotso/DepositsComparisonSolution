namespace DepositsComparisonDomainLogicAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DepositsComparison.Data.Public;
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
        private const decimal InterestTaxPercentage = 8;
        
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

        [HttpPost(nameof(GetFilteredDeposits))]
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

            var depositsResult = new List<DepositInfoWithPaymentPlan>();

            foreach (var deposit in deposits)
            {
                depositsResult.Add(new DepositInfoWithPaymentPlan
                {
                    Deposit = deposit,
                    PaymentPlan = GetPaymentPlan(request.Amount, request.PeriodInMonths, deposit)
                });
            }
            
            return new GetFilteredDepositsResponse
            {
                Deposits = depositsResult
            };
        }

        private PaymentPlan GetPaymentPlan(decimal amount, int months, DepositInfo deposit)
        {
            var relevantInterests = deposit.InterestOptions.Where(i => i.Months == months).ToList();
            
            if (relevantInterests.All(i => i.Type == InterestType.Fixed))
            {
                var relevantInterest = relevantInterests.FirstOrDefault(i => i.Months == months && i.Type == InterestType.Fixed);
                var paymentPlanEntries = new List<PaymentPlanEntry>();
                
                var interestAmount = amount * (relevantInterest.Percentage / 100);
                var interestTax = interestAmount * (InterestTaxPercentage / 100);
                var grossPaymentAmount = amount + interestAmount;
                var netPaymentAmount = grossPaymentAmount - interestTax;
                
                var totalInterestAmount = 0m; // To be used in case of multi year deposits
                var totalInterestTax = 0m; // To be used in case of multi year deposits
                for (int i = 1; i <= months; i++)
                {
                    if (months >= 12)
                    {
                        if (months % 12 == 0)
                        {
                            totalInterestAmount += interestAmount;
                            totalInterestTax += interestTax;
                            var currentPaymentAmount = amount + totalInterestAmount - totalInterestTax;
                            paymentPlanEntries.Add(new PaymentPlanEntry
                            {
                                Month = i,
                                InterestPercentage = relevantInterest.Percentage,
                                InterestAmount = totalInterestAmount,
                                InterestTax = totalInterestTax,
                                PaymentAmount = currentPaymentAmount,
                                DepositAmount = amount
                            });
                            grossPaymentAmount = amount + totalInterestAmount;
                            netPaymentAmount = grossPaymentAmount - totalInterestTax;
                            continue;
                        }
                    }
                    else
                    {
                        if (months == i)
                        {
                            totalInterestAmount += interestAmount;
                            totalInterestTax += interestTax;
                            paymentPlanEntries.Add(new PaymentPlanEntry
                            {
                                Month = i,
                                InterestPercentage = relevantInterest.Percentage,
                                InterestAmount = interestAmount,
                                InterestTax = interestTax,
                                PaymentAmount = netPaymentAmount,
                                DepositAmount = amount
                            });
                            continue;
                        }
                    }
                    
                    paymentPlanEntries.Add(new PaymentPlanEntry
                    {
                        Month = i,
                        InterestPercentage = 0,
                        InterestAmount = 0,
                        InterestTax = 0,
                        PaymentAmount = amount,
                        DepositAmount = amount
                    });
                }
                var effectiveAnnualInterest = CalculateEffectiveAnnualInterest(interestAmount, months); // ToDo: ???? Ефективна годишна лихва

                return new PaymentPlan
                {
                    DepositAmount = amount,
                    Months = months,
                    EffectiveAnnualInterest = effectiveAnnualInterest,
                    InterestTotalSum = totalInterestAmount,
                    InterestTotalTax = totalInterestTax,
                    GrossPaymentAmount = grossPaymentAmount,
                    NetPaymentAmount = netPaymentAmount,
                    Entries = paymentPlanEntries
                };
            }
            else
            {
                // ToDo: Cover logic for Variable interest
            }
            return new PaymentPlan
            {
                Entries = new List<PaymentPlanEntry>()
            };
        }

        private decimal CalculateEffectiveAnnualInterest(decimal interest, int months)
        {
            var n = months;
            var i = interest;
	
            var bracketsValue = 1 + (i/n);
            var onPowerOfN = Math.Pow((double)bracketsValue, n);
           return (decimal)(onPowerOfN - 1);
        }
    }
}