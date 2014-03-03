using System;

namespace XeroApi.Model
{
    public class Payment : EndpointModelBase
    {
        [ItemId]
        public Guid? PaymentID { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public string Reference { get; set; }

        public decimal? CurrencyRate { get; set; }

        public string PaymentType { get; set; }

        public string Status { get; set; }
        
        public bool IsReconciled { get; set; }

        [ItemUpdatedDate]
        public DateTime? UpdatedDateUTC { get; set; }

        public Account Account { get; set; }

        public Invoice Invoice { get; set; }
    }

    public class Payments : ModelList<Payment>
    {
    }
}
