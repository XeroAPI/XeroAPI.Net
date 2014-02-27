using System;

namespace XeroApi.Model
{
    public class Contact : EndpointModelBase
    {
        [ItemId]
        public Guid ContactID { get; set; }
        
        [ItemNumber]
        public string ContactNumber { get; set; }

        [ItemUpdatedDate]
        public DateTime? UpdatedDateUTC { get; set; }

        public string ContactStatus { get; set; }
        
        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string EmailAddress { get; set; }
        
        public string SkypeUserName { get; set; }

        public ContactPersons ContactPersons { get; set; }
        
        public string BankAccountDetails { get; set; }
        
        public string TaxNumber { get; set; }
        
        public string AccountsReceivableTaxType { get; set; }
        
        public string AccountsPayableTaxType { get; set; }
        
        public Addresses Addresses { get; set; }
        
        public Phones Phones { get; set; }
        
        public ContactGroups ContactGroups { get; set; }
        
        [ReadOnly]
        public bool IsSupplier { get; set; }

        [ReadOnly]
        public bool IsCustomer { get; set; }
        
        public string DefaultCurrency { get;  set; }

        public Balances Balances { get; set; }

        public BatchPayments BatchPayments { get; set; }

        public PaymentTerms PaymentTerms { get; set; }

        public override string ToString()
        {
            return string.Format("Contact:{0}", Name);
        }
    }

    public class Contacts : ModelList<Contact>
    {
    }
    
}