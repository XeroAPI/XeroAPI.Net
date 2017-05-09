using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeroApi.Model;
using XeroApi.Validation.Model;

namespace XeroApi.Validation.Helpers
{
    public static class TaxHelper
    {
        public static bool? IsValidTax(this LineItem li)
        {
            var taxAmount = CalculateTaxAmount(li);
            if (taxAmount != null)
            {
                var calculated = taxAmount.GetValueOrDefault();
                var liTax = li.TaxAmount.GetValueOrDefault();
                return calculated.NearlyEqualTo(liTax);                                
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
