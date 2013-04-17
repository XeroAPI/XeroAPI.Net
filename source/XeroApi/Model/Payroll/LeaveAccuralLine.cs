using System;

namespace XeroApi.Model.Payroll
{
    public class LeaveAccrualLine : EndpointModelBase
    {
        public Guid LeaveTypeID { get; set; }
        public decimal NumberOfUnits { get; set; }
        public bool AutoCalculate { get; set; }
    }

    public class LeaveAccrualLines : ModelList<LeaveAccrualLine>
    {
    }
}