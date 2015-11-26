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

        [ReadOnly]
        public string DefaultCurrency { get;  set; }

        [ReadOnly]
        public BatchPayments BatchPayments { get; set; }

        [ReadOnly]
        public Balances Balances { get; set; }

        public override string ToString()
        {
            return string.Format("Contact:{0}", Name);
        }
    }

    public class BatchPayments : ModelBase
    {
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string Details { get; set; }
    }

    public class Balances : ModelBase
    {
        public Balance AccountsReceivable { get; set; }
        public Balance AccountsPayable { get; set; }
    }

    public class Balance : ModelBase
    {
        public decimal Outstanding { get; set; }
        public decimal Overdue { get; set; }
    }


    public class Contacts : ModelList<Contact>
    {
    }
    
}