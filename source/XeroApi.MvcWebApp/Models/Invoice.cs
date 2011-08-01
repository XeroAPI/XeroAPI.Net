using System;

namespace Xero.ScreencastWeb.Models
{
    public class Invoice : ModelBase
    {
        public override string ApiEndpointName
        {
            get { return "Invoices"; }
        }

        public string InvoiceNumber;

        public Guid InvoiceID;

        public Contact Contact;

        public DateTime Date;

        public string Type;

        public DateTime DueDate;

        public string Reference;

        public string CurrencyCode;

        public string LineAmountTypes;

        public LineItem[] LineItems;

        public decimal SubTotal;

        public decimal TaxTotal;

        public decimal Total;

        public decimal AmountPaid;

        public decimal AmountDue;

        public decimal AmountCredited;

        public DateTime UpdatedDateUTC;
    }

    public class Invoices : ModelListBase<Invoice>
    {
        
    }
}