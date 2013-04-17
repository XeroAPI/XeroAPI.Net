using System;

namespace XeroApi.Model.Payroll
{
    public class LeaveApplication : HasUpdatedDate
    {
        public Guid LeaveApplicationID { get; set; }
        public Guid EmployeeID { get; set; }
        public Guid LeaveTypeID { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public LeavePeriods LeavePeriods { get; set; }
        public string Description { get; set; }
    }

    public class LeaveApplications : ModelList<LeaveApplication>
    {
    }
}
