namespace DepositsComparisonDomainLogicAPI.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DepositsComparison.Data.Public;
    using DepositsComparisonDomainLogic.Contracts.Models.Deposits;

    public class PaymentPlanGenerator : IPaymentPlanGenerator
    {
        private const decimal InterestTaxPercentage = 8;
        
        public PaymentPlan GeneratePaymentPlan(decimal amount, int months, DepositInfo deposit)
        {
            var relevantInterest = deposit.InterestOptions.FirstOrDefault(i => i.Months == months);

            if (relevantInterest == null)
            {
                return new PaymentPlan
                {
                    Entries = new List<PaymentPlanEntry>()
                };
            }
            
            var paymentPlanEntries = new List<PaymentPlanEntry>();
                
                var interestAmount = amount * (relevantInterest.Percentage / 100);
                var interestTax = interestAmount * (InterestTaxPercentage / 100);
                var grossPaymentAmount = amount + interestAmount;
                var netPaymentAmount = grossPaymentAmount - interestTax;
                
                var totalInterestAmount = 0m; // To be used in case of 12m+ deposits
                var totalInterestTax = 0m; // To be used in case of 12m+ deposits
                var amountForVariableInterest = amount;
                for (int i = 1; i <= months; i++)
                {
                    if (months >= 12)
                    {
                        if (months % 12 == 0)
                        {
                            if (relevantInterest.Type == InterestType.Fixed)
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
                            }
                            else
                            {
                                totalInterestAmount += amountForVariableInterest * (relevantInterest.Percentage / 100);
                                totalInterestTax = totalInterestAmount * (InterestTaxPercentage / 100);
                                var currentPaymentAmount = amountForVariableInterest + totalInterestAmount - totalInterestTax;
                                paymentPlanEntries.Add(new PaymentPlanEntry
                                {
                                    Month = i,
                                    InterestPercentage = relevantInterest.Percentage,
                                    InterestAmount = totalInterestAmount,
                                    InterestTax = totalInterestTax,
                                    PaymentAmount = currentPaymentAmount,
                                    DepositAmount = amountForVariableInterest
                                });
                                grossPaymentAmount = amountForVariableInterest + totalInterestAmount;
                                netPaymentAmount = grossPaymentAmount - totalInterestTax;
                                amountForVariableInterest += totalInterestAmount - totalInterestTax;
                            }
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
                
                var effectiveAnnualInterest = CalculateEffectiveAnnualInterest(interestAmount, months);

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