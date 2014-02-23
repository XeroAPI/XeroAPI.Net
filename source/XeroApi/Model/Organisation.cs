using System;

namespace XeroApi.Model
{
    public class Organisation : EndpointModelBase
    {
        public string Name;

        public string LegalName;

        public DateTime CreatedDateUTC;

        public string APIKey;

        public bool PaysTax;

        public string Version;

        public string OrganisationType;

        public string BaseCurrency;

        public string CountryCode;

        public bool IsDemoCompany;

        public DateTime? PeriodLockDate;

        public DateTime? EndOfYearLockDate;

        public string TaxNumber;

        public int FinancialYearEndDay;

        public int FinancialYearEndMonth;
        
        public string Timezone;
        
        public string ShortCode;

        public Addresses Addresses;

        public override string ToString()
        {
            return string.Format("Organisation:{0}", Name);
        }
    }

    public class Organisations : ModelList<Organisation>
    {
    }
}
