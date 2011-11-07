using System;

namespace XeroApi.Model
{
    public class Account : ModelBase
    {
        [ItemId]
        public Guid AccountID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public string TaxType { get; set; }

        public string Description { get; set; }

        public string Class { get; set; }

        public string SystemAccount { get; set; }

        public bool? EnablePaymentsToAccount { get; set; }

        public string BankAccountNumber { get; set; }

        public string CurrencyCode { get; set; }

        public string ReportingCode { get; set; }

        public string ReportingCodeName { get; set; }
    }

    
    public class Accounts : ModelList<Account>
    {
    }
}