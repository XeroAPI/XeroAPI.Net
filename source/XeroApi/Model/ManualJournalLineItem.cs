namespace XeroApi.Model
{
    public class ManualJournalLineItem : ModelBase
    {
        public string Description { get; set; }
        
        public string TaxType { get; set; }

        public decimal? TaxAmount { get; set; }
        
        public decimal? LineAmount { get; set; }

        public string AccountCode { get; set; }

        public TrackingCategories Tracking { get; set; }
    }
    
    public class ManualJournalLineItems : ModelList<ManualJournalLineItem>
    {
    }
}
