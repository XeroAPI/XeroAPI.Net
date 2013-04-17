using System;

namespace XeroApi.Model.Payroll
{
    public class LeaveEarningsLine : EndpointModelBase
    {
        public Guid EarningsRateID { get; set; }
        public decimal RatePerUnit { get; set; }
        public decimal NumberOfUnits { get; set; }
    }

    public class LeaveEarningsLines : ModelList<LeaveEarningsLine>
    {
    }
}