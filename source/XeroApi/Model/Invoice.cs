using System;

namespace XeroApi.Model
{
    public class Invoice : EndpointModelBase, IAttachmentParent
    {
        [ItemId]
        public virtual Guid InvoiceID { get; set; }

        [ItemNumber]
        public string InvoiceNumber { get; set; }

        [ItemUpdatedDate]
        public DateTime? UpdatedDateUTC { get; set; }

        public string Type { get; set; }

        public string Reference { get; set; }

        [ReadOnly]
        public Payments Payments { get; set; }

        [ReadOnly]
        public CreditNotes CreditNotes { get; set; }

        [ReadOnly]
        public decimal? AmountDue { get; set; }

        [ReadOnly]
        public decimal? AmountPaid { get; set; }

        [ReadOnly]
        public decimal? AmountCredited { get; set; }
        
        public string Url { get; set; }

        [ReadOnly]
        public string ExternalLinkProviderName { get; set; }

        [ReadOnly]
        public bool? SentToContact { get; set; }

        [ReadOnly]
        public bool? HasAttachments { get; set; }

        public decimal? CurrencyRate { get; set; }

        public Contact Contact { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? DueDate { get; set; }

        public virtual Guid? BrandingThemeID { get; set; }

        public virtual string Status { get; set; }

        public LineAmountType LineAmountTypes { get; set; }

        public virtual LineItems LineItems { get; set; }

        public virtual decimal? SubTotal { get; set; }

        public virtual decimal? TotalDiscount { get; set; }

        public virtual decimal? TotalTax { get; set; }

        public virtual decimal? Total { get; set; }

        public virtual string CurrencyCode { get; set; }

        [ReadOnly]
        public DateTime? FullyPaidOnDate { get; set; }

        public DateTime? ExpectedPaymentDate { get; set; }

        public DateTime? PlannedPaymentDate { get; set; }

        public override string ToString()
        {
            return string.Format("Invoice:{0} Id:{1}", InvoiceNumber, InvoiceID);
        }
    }
   
   
    public class Invoices : ModelList<Invoice>
    {
    }
}