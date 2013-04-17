using System;
using System.Runtime.Serialization;

namespace XeroApi.Model.Payroll
{
    public class Payslip : EndpointModelBase
    {
        public Guid PayslipID { get; set; }
        public Guid EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeGroup { get; set; }        
        public DateTime? LastEdited { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetPay { get; set; }
        public decimal Reimbursements { get; set; }
        public decimal Super { get; set; }
        public decimal Tax { get; set; }
        public decimal Wages { get; set; }

        public DeductionLines DeductionLines { get; set; }
        public EarningsLines EarningsLines { get; set; }
        public LeaveEarningsLines LeaveEarningsLines { get; set; }
        public TimesheetEarningsLines TimesheetEarningsLines { get; set; }
        public LeaveAccrualLines LeaveAccrualLines { get; set; }
        public ReimbursementLines ReimbursementLines { get; set; }
        public SuperannuationLines SuperannuationLines { get; set; }
        public TaxLines TaxLines { get; set; }
    }

    public class Payslips : ModelList<Payslip>
    {
    }
}