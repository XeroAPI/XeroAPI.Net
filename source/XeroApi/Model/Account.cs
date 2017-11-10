using System;

namespace XeroApi.Model
{
    public class Account : EndpointModelBase
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

        /// <summary>
        /// This is an important property because Accounts that represent credit cards will
        /// have a Type=BANK, Class=ASSET. The only way that we can map a credit card account
        /// to a liablility isto check that BankAccountType=CREDITCARD.
        /// </summary>
        public string BankAccountType { get; set; }

        // Added for v2.14
        public bool ShowInExpenseClaims { get; set; }

        public override string ToString()
        {
            return string.Format("Account:{0}", Code);
        }
    }

    
    public class Accounts : ModelList<Account>
    {
    }
}