using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeroApi.Model;

namespace XeroApi.Validation.Helpers
{
    public static class LineItemHelper
    {
        public static decimal GetTotal(this LineItem li)
        {
            return GetSubTotal(li) + li.TaxAmount.GetValueOrDefault();
        }

        public static decimal GetSubTotal(this LineItem li)
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

        public static decimal GetSubTotal(this IEnumerable<LineItem> li)
        {
            return li.Sum(a => a.GetSubTotal());
        }

        public static decimal GetTotal(this IEnumerable<LineItem> li)
        {
            return li.Sum(a => a.GetTotal());
        }
    }
}
