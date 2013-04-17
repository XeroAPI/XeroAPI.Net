using System;
using XeroApi.Model.Payroll.Enums;

namespace XeroApi.Model.Payroll
{
    public class EarningsRate : HasUpdatedDate
    {
        public Guid EarningsRateID { get; set; }
        public string Name { get; set; }
        public EarningsType EarningsType { get; set; }
        public RateType RateType { get; set; }
        public string AccountCode { get; set; }
        public string TypeOfUnits { get; set; }
        public bool IsExemptFromTax { get; set; }
        public bool IsExemptFromSuper { get; set; }
        public decimal? RatePerUnit { get; set; }
        public decimal? Multiplier { get; set; }
        public decimal? Amount { get; set; }
        public bool AccrueLeave { get; set; }        
    }

    public class EarningsRates : ModelList<EarningsRate>
    {
    }
}