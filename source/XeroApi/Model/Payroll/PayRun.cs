using System;
using XeroApi.Model.Payroll.Enums;

namespace XeroApi.Model.Payroll
{
    public class PayRun : HasUpdatedDate
    {
        public Guid PayRunID { get; set; }
        public Guid PayrollCalendarID { get; set; }
        public DateTime? PayRunPeriodStartDate { get; set; }
        public DateTime? PayRunPeriodEndDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PayslipMessage { get; set; }
        public bool Unscheduled { get; set; }
        public decimal Wages { get; set; }
        public decimal Deductions { get; set; }
        public decimal Tax { get; set; }
        public decimal Super { get; set; }
        // Should this change to Reimbursements?
        public decimal Reimbursement { get; set; }
        public decimal NetPay { get; set; }
        public PayRunStatus PayRunStatus { get; set; }

        public Payslips Payslips { get; set; }        
    }

    public class PayRuns : ModelList<PayRun>
    {
    }
}