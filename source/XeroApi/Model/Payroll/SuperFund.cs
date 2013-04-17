using System;
using XeroApi.Model.Payroll.Enums;

namespace XeroApi.Model.Payroll
{
    public class SuperFund : HasUpdatedDate
    {
        public Guid SuperFundID { get; set; }
        public long ABN { get; set; }
        public string SPIN { get; set; }
        public SuperfundType Type { get; set; }
        public string AccountName { get; set; }
        public long AccountNumber { get; set; }
        public int BSB { get; set; }
        public long EmployerNumber { get; set; }
        public string Name { get; set; }
    }

    public class SuperFunds : ModelList<SuperFund>
    {
    }
}