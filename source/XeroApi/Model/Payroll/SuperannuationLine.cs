using System;
using XeroApi.Model.Payroll.Enums;

namespace XeroApi.Model.Payroll
{
    public class SuperannuationLine : EndpointModelBase
    {
        public Guid SuperMembershipID { get; set; }
        public SuperannuationContribution ContributionType { get; set; }
        public SuperannuationCalculation CalculationType { get; set; }
        public decimal MinimumMonthlyEarnings { get; set; }
        public int ExpenseAccountCode { get; set; }
        public int LiabilityAccountCode { get; set; }
        public DateTime? PaymentDateForThisPeriod { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? Amount { get; set; }
    }

    public class SuperannuationLines : ModelList<SuperannuationLine>
    {
    }
}