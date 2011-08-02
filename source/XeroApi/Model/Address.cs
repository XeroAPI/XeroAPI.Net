using System;

namespace XeroApi.Model
{
    public class Address : ModelBase
    {
        public string AddressType { get; set; }
        
        public string AddressLine1 { get; set; }
        
        public string AddressLine2 { get; set; }
        
        public string AddressLine3 { get; set; }
        
        public string AddressLine4 { get; set; }
        
        public string City { get; set; }
        
        public string Region { get; set; }
        
        public string PostalCode { get; set; }
        
        public string Country { get; set; }

        public string AttentionTo { get; set; }
    }

    
    public class Addresses : ModelList<Address>
    {
    }
}