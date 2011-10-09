using System;

namespace XeroApi.Model
{
    public class Organisation : ModelBase
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
    }

    public class Organisations : ModelList<Organisation>
    {
    }
}
