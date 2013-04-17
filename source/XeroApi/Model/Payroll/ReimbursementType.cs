using System;

namespace XeroApi.Model.Payroll
{
    public class ReimbursementType : HasUpdatedDate
    {
        public Guid ReimbursementTypeID { get; set; }
        public string Name { get; set; }
        public int AccountCode { get; set; }        
    }

    public class ReimbursementTypes : ModelList<ReimbursementType>
    {
    }
}