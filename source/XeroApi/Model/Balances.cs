using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XeroApi.Model
{
    public class Balances
    {
        public AccountBase AccountsReceivable { get; set; }

        public AccountBase AccountsPayable { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Sales Invoices: {0} ", AccountsReceivable.ToString());

            sb.AppendFormat("Bills: {0}", AccountsPayable.ToString());

            return sb.ToString();
        }
    }
}
