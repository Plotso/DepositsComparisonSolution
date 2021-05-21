﻿namespace DepositsComparisonDomainLogic.Contracts.Models.Deposits
{
    using DepositsComparison.Data.Public;

    public class InterestInfo
    {
        public int Months { get; set; }
        
        public decimal Percentage { get; set; }
        
        public InterestType Type { get; set; }
    }
}