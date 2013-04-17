using System;

namespace XeroApi.Model.Payroll
{
    public class ReimbursementLine : EndpointModelBase
    {
        public Guid ReimbursementTypeID { get; set; }
        public decimal? Amount { get; set; }
        public string Description { get; set; }
        public string ExpenseAccount { get; set; }
    }

    public class ReimbursementLines : ModelList<ReimbursementLine>
    {
    }
}