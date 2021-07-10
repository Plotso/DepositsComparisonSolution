using DepositsComparisonDomainLogic.Contracts.Models.Deposits;
using System.Collections.Generic;

namespace DepositsComparer
{
    public interface IFilteredDepositCollection
    {
        public void SetDeposits(IEnumerable<DepositInfoWithPaymentPlan> depositInfoWithPaymentPlans);

        public IEnumerable<DepositInfoWithPaymentPlan> GetAll();

        public DepositInfoWithPaymentPlan GetByDepositName(string id);

    }
}