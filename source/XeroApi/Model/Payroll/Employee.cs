using System;
using XeroApi.Model.Payroll.Enums;

namespace XeroApi.Model.Payroll
{
    public class Employee : HasUpdatedDate
    {
        public Guid EmployeeID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public HomeAddress HomeAddress { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string TwitterUserName { get; set; }
        public string Email { get; set; }
        
        public DateTime? StartDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public EmploymentStatus Status { get; set; }
        
        public string Classification { get; set; }
        public string EmployeeGroupName { get; set; }
        public string Occupation { get; set; }
        
        public bool IsAuthorisedToApproveLeave { get; set; }
        public bool IsAuthorisedToApproveTimesheets { get; set; }
        
        public Guid? OrdinaryEarningsRateID { get; set; }
        public string OrdinaryEarningsRateName { get; set; }
        
        public Guid? PayrollCalendarID { get; set; }
        public string PayrollCalendarName { get; set; }

        public BankAccounts BankAccounts { get; set; }
        public PayTemplate PayTemplate { get; set; }        
        public OpeningBalances OpeningBalances { get; set; }
        public SuperMemberships SuperMemberships { get; set; }        
    }

    public class Employees : ModelList<Employee>
    {
    }
}
