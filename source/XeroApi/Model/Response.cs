using System;
using System.Reflection;
using System.Xml.Serialization;

namespace XeroApi.Model
{
    [XmlRoot(ElementName="Response", Namespace="")]
    [Serializable]
    public class Response : ResponseBase
    {
        public Accounts Accounts { get; set; }
        public BankTransactions BankTransactions { get; set; }
        public BrandingThemes BrandingThemes { get; set; }
        public Contacts Contacts { get; set; }
        public CreditNotes CreditNotes { get; set; }
        public Currencies Currencies { get; set; }
        public Employees Employees { get; set; }
        public Invoices Invoices { get; set; }
        public Items Items { get; set; }
        public Journals Journals { get; set; }
        public ManualJournals ManualJournals { get; set; }
        public Organisations Organisations { get; set; }
        public Payments Payments { get; set; }
        public Reports Reports { get; set; }
        public TaxRates TaxRates { get; set; }
        public TrackingCategories TrackingCategories { get; set; }
        public Attachments Attachments { get; set; }
        public Users Users { get; set; }
        public Receipts Receipts { get; set; }
        public ExpenseClaims ExpenseClaims { get; set; }        
    }
}
