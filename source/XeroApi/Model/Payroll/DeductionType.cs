using System;

namespace XeroApi.Model.Payroll
{
    public class DeductionType : HasUpdatedDate
    {
        public Guid DeductionTypeID { get; set; }
        public string Name { get; set; }
        public int AccountCode { get; set; }
        public bool ReducesTax { get; set; }
        public bool ReducesSuper { get; set; }        
    }

    public class DeductionTypes : ModelList<DeductionType>
    {        
    }
}
