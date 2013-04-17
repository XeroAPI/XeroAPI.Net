using System;
using System.Xml.Serialization;

namespace XeroApi.Model.Payroll
{
    [XmlRoot(ElementName = "Response", Namespace = "")]
    [Serializable]
    public class Response : ResponseBase
    {        
        public Employees Employees { get; set; }
        public PayItems PayItems { get; set; }
        public PayrollCalendars PayrollCalendars { get; set; }
        public LeaveApplications LeaveApplications { get; set; }
        public PayRuns PayRuns { get; set; }
        public SuperFunds SuperFunds { get; set; }
        public Timesheets Timesheets { get; set; }

        public Payslip Payslip { get; set; }
        public TaxDeclaration TaxDeclaration { get; set; }        
    }
}
