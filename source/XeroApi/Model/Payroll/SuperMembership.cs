using System;

namespace XeroApi.Model.Payroll
{
    public class SuperMembership : EndpointModelBase
    {
        public Guid SuperMembershipID { get; set; }
        public Guid SuperFundID { get; set; }
        public int EmployeeNumber { get; set; }
    }

    public class SuperMemberships : ModelList<SuperMembership>
    {
    }
}