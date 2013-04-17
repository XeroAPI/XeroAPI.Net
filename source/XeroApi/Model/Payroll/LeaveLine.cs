using System;
using XeroApi.Model.Payroll.Enums;

namespace XeroApi.Model.Payroll
{
    public class LeaveLine : EndpointModelBase
    {
        public Guid LeaveTypeID { get; set; }
        public decimal NumberOfUnits { get; set; }
        public decimal AnnualNumberOfUnits { get; set; }
        public LeaveCalculation CalculationType { get; set; }
        public decimal FullTimeNumberOfUnitsPerPeriod { get; set; }
    }

    public class LeaveLines : ModelList<LeaveLine>
    {
    }
}