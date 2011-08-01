using System;

namespace Xero.ScreencastWeb.Models
{
    public class Contact : ModelBase
    {
        public override string ApiEndpointName
        {
            get { return "Contacts"; }
        }

        public Guid ContactID;	                    //Xero identifier
        public ContactStatus ContactStatus;	        //Current status of a contact – see contact status types
        public string Name;	                        //Name of contact organisation (max length = 500)
        public string FirstName;	                //First name of contact person (max length = 255)
        public string LastName;	                    //Last name of contact person (max length = 255)
        public string EmailAddress;	                //Email address of contact person (umlauts not supported) (max length = 500)
        public string BankAccountDetails;	        //Bank account number of contact
        public string TaxNumber;	                //Tax number of contact
        public object AccountsReceivableTaxType;	//Default tax type used for contact on AR invoices
        public object AccountsPayableTaxType;	    //Default tax type used for contact on AP invoices
        public Addresses Addresses;	                //Store certain address types for a contact – see address types
        public Phones Phones;	                    //Store certain phone types for a contact – see phone types
        public DateTime UpdatedDateUTC;	            //UTC timestamp of last update to contact
        public ContactGroups ContactGroups;	        //Displays which contact groups a contact is included in
        public bool IsSupplier;	                    //true or false – Boolean that describes if a contact that has any AP invoices entered against them
        public bool IsCustomer;	                    //true or false – Boolean that describes if a contact has any AR invoices entered against them
        public string DefaultCurrency;	            //Default currency for raising invoices against contact
    }

    public class Contacts : ModelListBase<Contact>
    {
    }

}
