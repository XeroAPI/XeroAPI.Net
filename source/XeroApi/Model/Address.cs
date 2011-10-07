using System;
using System.Text;

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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(AttentionTo))
                sb.Append("Attn: " + AttentionTo + Environment.NewLine);

            if (!string.IsNullOrEmpty(AddressLine1))
                sb.Append(AddressLine1 + Environment.NewLine);

            if (!string.IsNullOrEmpty(AddressLine2))
                sb.Append(AddressLine2 + Environment.NewLine);

            if (!string.IsNullOrEmpty(AddressLine3))
                sb.Append(AddressLine3 + Environment.NewLine);

            if (!string.IsNullOrEmpty(AddressLine4))
                sb.Append(AddressLine4 + Environment.NewLine);
            
            if (!string.IsNullOrEmpty(City))
                sb.Append(City + Environment.NewLine);

            if (!string.IsNullOrEmpty(Region))
                sb.Append(Region + Environment.NewLine);

            if (!string.IsNullOrEmpty(PostalCode))
                sb.Append(PostalCode + Environment.NewLine);

            if (!string.IsNullOrEmpty(Country))
                sb.Append(Country + Environment.NewLine);

            return sb.ToString().TrimEnd(' ');

        }
    }

    
    public class Addresses : ModelList<Address>
    {
    }
}