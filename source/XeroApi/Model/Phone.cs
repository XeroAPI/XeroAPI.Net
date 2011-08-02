using System;

namespace XeroApi.Model
{
    public class Phone : ModelBase
    {
        public string PhoneType { get; set; }

        public string PhoneNumber { get; set; }

        public string PhoneAreaCode { get; set; }

        public string PhoneCountryCode { get; set; }
    }
    
    public class Phones : ModelList<Phone>
    {
    }
}