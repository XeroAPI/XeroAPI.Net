using System;

namespace XeroApi.Model
{
    public class CreditNote : EndpointModelBase
    {
        [ItemId]
        public Guid CreditNoteID { get; set; }

        [ItemNumber]
        public string CreditNoteNumber { get; set; }

        [ItemUpdatedDate]
        public DateTime? UpdatedDateUTC { get; set; }

        public string Type { get; set; }

        public string Reference { get; set; }
        
        public bool? SentToContact { get; set; }

        public decimal? AppliedAmount { get; set; }
        
        public decimal RemainingCredit { get; set; }

        public decimal? CurrencyRate { get; set; }

        public Contact Contact { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? DueDate { get; set; }

        public virtual Guid? BrandingThemeID { get; set; }

        public virtual string Status { get; set; }

        public LineAmountType LineAmountTypes { get; set; }

        public virtual LineItems LineItems { get; set; }

        public virtual decimal? SubTotal { get; set; }

        public virtual decimal? TotalTax { get; set; }

        public virtual decimal? Total { get; set; }

        public virtual string CurrencyCode { get; set; }

        public DateTime? FullyPaidOnDate { get; set; }

        public Allocations Allocations { get; set; }
    }
    
    public class CreditNotes : ModelList<CreditNote>
    {
    }

    public class Allocation : EndpointModelBase
    {
        public decimal AppliedAmount { get; set; }
        public DateTime Date { get; set; }
        public Invoice Invoice { get; set; }
    }

    public class Allocations : ModelList<Allocation>
    {
    }

}
