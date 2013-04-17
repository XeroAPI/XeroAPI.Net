using System;
using XeroApi.Model.Payroll.Enums;

namespace XeroApi.Model.Payroll
{
    public class LeavePeriod : EndpointModelBase
    {
        public DateTime? PayPeriodStartDate { get; set; }
        public DateTime? PayPeriodEndDate { get; set; }
        public LeavePeriodStatus LeavePeriodStatus { get; set; }
        public decimal NumberOfUnits { get; set; }
    }

    public class LeavePeriods : ModelList<LeavePeriod>
    {
    }
}
