using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XeroApi.Model
{
    public class BatchPayments
    {
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string Details { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Bank Account Number: {0} ", BankAccountNumber);

            sb.AppendFormat("Bank Account Name: {0} ", BankAccountName);

            sb.AppendFormat("Details: {0}", Details);

            return sb.ToString();
        }
    }
}
