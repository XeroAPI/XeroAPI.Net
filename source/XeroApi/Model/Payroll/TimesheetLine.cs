using System;

namespace XeroApi.Model.Payroll
{
    public class TimesheetLine : HasUpdatedDate
    {
        public Guid EarningsRateID { get; set; }
        public Guid TrackingItemID { get; set; }
        public NumberOfUnits NumberOfUnits { get; set; }        
    }

    public class TimesheetLines : ModelList<TimesheetLine>
    {
    }

    public class NumberOfUnits : ModelList<NumberOfUnit>
    {
    }

    public class NumberOfUnit : EndpointModelBase
    {
    }
}