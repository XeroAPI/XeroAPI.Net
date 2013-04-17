using System;
using XeroApi.Model.Payroll.Enums;

namespace XeroApi.Model.Payroll
{
    public class EarningsLine : EndpointModelBase
    {
        public Guid EarningsRateID { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AnnualSalary { get; set; }
        public decimal? RatePerUnit { get; set; }
        public decimal? NormalNumberOfUnits { get; set; }
        public decimal? NumberOfUnitsPerWeek { get; set; }
        public decimal? NumberOfUnits { get; set; }
        public EarningsRateCalculation EarningsRateCalculation { get; set; }
    }

    public class EarningsLines : ModelList<EarningsLine>
    {
    }    
}