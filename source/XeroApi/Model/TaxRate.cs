namespace XeroApi.Model
{
    public class TaxRate : EndpointModelBase
    {
        
        public string Name { get; set; }

        public string TaxType { get; set; }

        public bool CanApplyToAssets { get; set; }

        public bool CanApplyToEquity { get; set; }

        public bool CanApplyToExpenses { get; set; }

        public bool CanApplyToLiabilities { get; set; }

        public bool CanApplyToRevenue { get; set; }

        public decimal? DisplayTaxRate { get; set; }

        public decimal? EffectiveRate { get; set; }

        public string Status { get; set; }

        public virtual TaxComponents TaxComponents { get; set; }
    }

    public class TaxRates : ModelList<TaxRate>
    {
    }

    

}