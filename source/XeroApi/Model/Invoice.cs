using System;

namespace XeroApi.Model
{
    public class Invoice : TradingTransaction
    {
        [ItemId]
        public virtual Guid InvoiceID { get; set; }

        [ItemNumber]
        public string InvoiceNumber { get; set; }

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

        public override string ToString()
        {
            return string.Format("Invoice:{0} Id:{1}", InvoiceNumber, InvoiceID);
        }
    }
   
   
    public class Invoices : ModelList<Invoice>
    {
    }
}