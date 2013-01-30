using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeroApi.Model;
using XeroApi.Validation.Model;

namespace XeroApi.Validation.Helpers
{
    static class TaxHelper
    {
        public static bool? IsValidTax(this LineItem li)
        {
            var taxAmount = CalculateTaxAmount(li);
            if (taxAmount != null)
            {
                return (taxAmount == li.TaxAmount);
            }

            return null;
        }

        public static decimal? CalculateTaxAmount(this LineItem li)
        {
            var taxRate = TaxCodeData.TaxCodes
                        .Where(a => a.Country == Country.UK)
                        .Where(a => a.TaxType.ToString() == li.TaxType)
                        .FirstOrDefault();

            if (taxRate != null)
            {
                var rate = taxRate.Rate / 100d;
                var taxAmount = li.LineAmount.GetValueOrDefault() * (decimal)rate;
                return taxAmount;
            }
            return null;
        }

    }
}
