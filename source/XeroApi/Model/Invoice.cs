using System;

namespace XeroApi.Model
{
    public class Invoice : ModelBase
    {

        [ItemId]
        public Guid InvoiceID { get; set; }

        [ItemNumber]
        public string InvoiceNumber { get; set; }

        public DateTime Date { get; set; }

        public DateTime DueDate { get; set; }

        [ItemUpdatedDate]
        public DateTime UpdatedDateUTC { get; set; }

        public Contact Contact { get; set; }

        public string Status { get; set; }

        public decimal SubTotal { get; set; }

        public decimal TotalTax { get; set; }

        public decimal Total { get; set; }

        public Guid BrandingThemeID { get; set; }

        public decimal AmountPaid { get; set; }

    }
}
