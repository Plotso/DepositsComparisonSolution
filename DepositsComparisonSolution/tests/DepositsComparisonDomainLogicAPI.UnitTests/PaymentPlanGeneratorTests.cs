namespace DepositsComparisonDomainLogicAPI.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using DepositsComparison.Data.Public;
    using DepositsComparisonDomainLogic.Contracts.Models.Deposits;
    using FluentAssertions;
    using NUnit.Framework;
    using Services.Interfaces;

    [TestFixture]
    public class PaymentPlanGeneratorTests
    {
        private const decimal DefaultTestAmount = 5000;
        private const int DefaultMonths = 12;
        
        private IPaymentPlanGenerator _paymentPlanGenerator;

        [OneTimeSetUp]
        public void Setup()
        {
            _paymentPlanGenerator = new PaymentPlanGenerator();
        }

        [Test]
        public void GeneratePaymentPlan_WithNoInterestsInDeposit_ShouldReturnEmptyPaymentPlan()
        {
            var result = _paymentPlanGenerator.GeneratePaymentPlan(DefaultTestAmount, DefaultMonths, GetDepositInfoWithEmptyInterestOptions());

            result.Should().NotBeNull();
            result.Entries.Should().BeEmpty();
        }

        [Test]
        public void GeneratePaymentPlan_WithNotRelevantInterestsInDeposit_ShouldReturnEmptyPaymentPlan()
        {
            var deposit = GetDepositInfoWithEmptyInterestOptions();
            SetInterest(deposit, false);
            var result = _paymentPlanGenerator.GeneratePaymentPlan(DefaultTestAmount, DefaultMonths, deposit);

            result.Should().NotBeNull();
            result.Entries.Should().BeEmpty();
        }

        [Test]
        public void GeneratePaymentPlan_WithRelevantInterestsInDeposit_ShouldNotReturnEmptyPaymentPlan()
        {
            var deposit = GetDepositInfoWithEmptyInterestOptions();
            SetInterest(deposit);
            var result = _paymentPlanGenerator.GeneratePaymentPlan(DefaultTestAmount, DefaultMonths, deposit);

            result.Should().NotBeNull();
            result.Entries.Should().NotBeEmpty();
        }

        [Test]
        public void GeneratePaymentPlan_WithRelevantInterestsInDeposit_ShouldReturnCorrectPaymentPlan()
        {
            var deposit = GetDepositInfoWithEmptyInterestOptions();
            SetInterest(deposit);
            var result = _paymentPlanGenerator.GeneratePaymentPlan(DefaultTestAmount, DefaultMonths, deposit);
            var expectedInterestAmount = DefaultTestAmount * (deposit.InterestOptions.First().Percentage / 100);
            var expectedGrossPayment = DefaultTestAmount + expectedInterestAmount;
            var expectedTax = (decimal)0.08 * expectedInterestAmount;
            var expectedNetAmount = expectedGrossPayment - expectedTax;

            result.Should().NotBeNull();
            result.DepositAmount.Should().Be(DefaultTestAmount);
            result.InterestTotalSum.Should().Be(expectedInterestAmount);
            result.GrossPaymentAmount.Should().Be(expectedGrossPayment);
            result.NetPaymentAmount.Should().Be(expectedNetAmount);
            result.Months.Should().Be(DefaultMonths);
            result.InterestTotalTax.Should().Be(expectedTax);
            result.Entries.Should().NotBeEmpty();
            result.Entries.Count().Should().Be(DefaultMonths);
            for (int i = 0; i < DefaultMonths - 1; i++)
            {
                result.Entries.ToList()[i].InterestAmount.Should().Be(0);
                result.Entries.ToList()[i].InterestPercentage.Should().Be(0);
                result.Entries.ToList()[i].InterestTax.Should().Be(0);
                result.Entries.ToList()[i].PaymentAmount.Should().Be(DefaultTestAmount);
                result.Entries.ToList()[i].DepositAmount.Should().Be(DefaultTestAmount);
                result.Entries.ToList()[i].Month.Should().Be(i + 1);
            }

            result.Entries.Last().Month.Should().Be(DefaultMonths);
            result.Entries.Last().InterestAmount.Should().Be(expectedInterestAmount);
            result.Entries.Last().InterestPercentage.Should().Be(deposit.InterestOptions.First().Percentage);
            result.Entries.Last().InterestTax.Should().Be(expectedTax);
            result.Entries.Last().PaymentAmount.Should().Be(expectedNetAmount);
            result.Entries.Last().DepositAmount.Should().Be(DefaultTestAmount);

        }

        private DepositInfo GetDepositInfoWithEmptyInterestOptions() =>
            new()
            {
                Bank = new BankInfo {Name = "TestBank"},
                MinAmount = DefaultTestAmount - 1,
                MaxAmount = DefaultTestAmount + 1,
                Currency = Currency.BGN,
                InterestOptions = new List<InterestInfo>()
            };

        private void SetInterest(DepositInfo deposit, bool isRelevant = true)
        {
            deposit.InterestOptions = new List<InterestInfo>
            {
                new()
                {
                    Months = isRelevant ? DefaultMonths : DefaultMonths * 2,
                    Percentage = 3,
                    Type = InterestType.Fixed
                }
            };
        }
    }
}