using System;

namespace XeroApi.Model
{
    public class BankTransaction : ModelBase
    {
        [ItemId]
        public Guid BankTransactionID { get; set; }

        public Account BankAccount { get; set; }

        public string Type { get; set; }

        public string Reference { get; set; }
        
        public string Url { get; set; }

        public string ExternalLinkProviderName { get; set; }

        public bool IsReconciled { get; set; }

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

        [ItemUpdatedDate]
        public DateTime? UpdatedDateUTC { get; set; }

        public virtual string CurrencyCode { get; set; }

        public DateTime? FullyPaidOnDate { get; set; }
    }


    public class BankTransactions : ModelList<BankTransaction>
    {
    }
}