using System;

namespace XeroApi.Model.Payroll
{
    public class TaxLine : EndpointModelBase
    {
        public Guid PayslipTaxLineID { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public string LiabilityAccount { get; set; }        
    }

    public class TaxLines : ModelList<TaxLine>
    {
    }
}