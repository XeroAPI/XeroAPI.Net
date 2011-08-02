using System;

namespace XeroApi.Model
{
    public class JournalLine : ModelBase
    {
        public Guid JournalLineID { get; set; }

        public Guid AccountID { get; set; }

        public string AccountCode { get; set; }
        
        public string AccountType { get; set; }

        public string AccountName { get; set; }

        public string Description { get; set; }

        public decimal NetAmount { get; set; }

        public decimal GrossAmount { get; set; }

        public decimal TaxAmount { get; set; }

        public string TaxType { get; set; }
        
        public string TaxName { get; set; }
        
        public TrackingCategories TrackingCategories { get; set; }
    }
    

    public class JournalLines : ModelList<JournalLine>
    {
    }

}
