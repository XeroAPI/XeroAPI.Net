using System;

namespace XeroApi.Model.Payroll
{
    public class LeaveType : HasUpdatedDate
    {
        public Guid LeaveTypeID { get; set; }
        public string Name { get; set; }
        public string TypeOfUnits { get; set; }
        public decimal NormalEntitlement { get; set; }
        public decimal LeaveLoadingRate { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool ShowOnPayslip { get; set; }        
    }

    public class LeaveTypes : ModelList<LeaveType>
    {
    }
}