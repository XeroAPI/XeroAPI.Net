using System;

namespace XeroApi.Model.Payroll
{
    public class Timesheet : EndpointModelBase
    {
        public Guid TimesheetID { get; set; }
        public Guid EmployeeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public decimal Hours { get; set; }
        public TimesheetLines TimesheetLines { get; set; }        
    }

    public class Timesheets : ModelList<Timesheet>
    {
    }
}