using System;
using System.Text;

namespace XeroApi.Model
{
    public class Phone : ModelBase
    {
        public string PhoneType { get; set; }

        public string PhoneNumber { get; set; }

        public string PhoneAreaCode { get; set; }

        public string PhoneCountryCode { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(PhoneCountryCode))
            {
                sb.Append(string.Format("(+{0}) ", PhoneCountryCode));
            }

            if (!string.IsNullOrEmpty(PhoneAreaCode))
            {
                sb.Append(PhoneAreaCode + " ");
            }

            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                sb.Append(PhoneNumber);
            }

            return sb.ToString().TrimEnd(' ');
        }
    }
    
    public class Phones : ModelList<Phone>
    {
    }
}