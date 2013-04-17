using System;
using XeroApi.Model.Payroll.Enums;

namespace XeroApi.Model.Payroll
{
    public class DeductionLine : EndpointModelBase
    {
        public Guid DeductionTypeID { get; set; }
        public decimal? Amount { get; set; }
        public DeductionCalculation CalculationType { get; set; }
        public decimal NumberOfUnits { get; set; }
        public decimal Percentage { get; set; }
    }

    public class DeductionLines : ModelList<DeductionLine>
    {
    }    
}
