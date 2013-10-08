using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XeroApi.Model
{
    public class PaymentTerms
    {
        public PaymentTermBase Bills { get; set; }
        public PaymentTermBase Sales { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Bills: " + Bills.ToString() + " ");

            sb.Append("Sales: " + Sales.ToString());

            return sb.ToString();
        }
    }
}
