using DepositsComparisonDomainLogic.Contracts;
using DepositsComparisonDomainLogic.Contracts.Models.Deposits;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DepositsComparer
{
    public class FilteredDepositCollection : IFilteredDepositCollection
    {
        private IEnumerable<DepositInfoWithPaymentPlan> _filteredDepositsWithPaymentPlan;

        public FilteredDepositCollection()
        {
            _filteredDepositsWithPaymentPlan = new List<DepositInfoWithPaymentPlan>();
        }
        public void SetDeposits(IEnumerable<DepositInfoWithPaymentPlan> depositInfoWithPaymentPlans)
        {
            _filteredDepositsWithPaymentPlan = depositInfoWithPaymentPlans;
        }

        public IEnumerable<DepositInfoWithPaymentPlan> GetAll()
        {
            return _filteredDepositsWithPaymentPlan;
        }
        public DepositInfoWithPaymentPlan GetByDepositName(string name)
        {
            return _filteredDepositsWithPaymentPlan.FirstOrDefault(X => X.Deposit.Name == name);
        }
    }
}