using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeroApi.Model;

namespace XeroApi.Validation.Helpers
{
    static class LineItemHelper
    {
        public static decimal GetTotal(this LineItem li)
        {
            if (li.LineAmount.HasValue)
            {
                return li.LineAmount.Value;
            }
            else
            {
                return li.UnitAmount.GetValueOrDefault() * li.Quantity.GetValueOrDefault();
            }
        }
    }
}
