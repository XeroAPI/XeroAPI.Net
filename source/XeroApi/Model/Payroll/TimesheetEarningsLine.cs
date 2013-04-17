using System;

namespace XeroApi.Model.Payroll
{
    public class TimesheetEarningsLine : EndpointModelBase
    {
        public Guid EarningsRateID { get; set; }
        public decimal? RatePerUnit { get; set; }
        public decimal? Amount { get; set; }
    }

    public class TimesheetEarningsLines : ModelList<TimesheetEarningsLine>
    {
    }
}