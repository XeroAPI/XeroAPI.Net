using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeroApi.Model;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using XeroApi.Validation.Helpers;

namespace XeroApi.Validation
{
    public class InvoiceValidator : Validator<Invoice>
    {
        Validator<LineItem> lineItemValidator = null;

        public InvoiceValidator(Validator<LineItem> lineItemValidator)
            : base(null, null)
        {
            this.lineItemValidator = lineItemValidator;
        }

        protected override void DoValidate(Invoice objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate.Contact == null)
            {
                validationResults.AddResult(new ValidationResult("The document has no Contact", currentTarget, key, "Contact", this));
            }

            if (objectToValidate.LineItems == null)
            {
                validationResults.AddResult(new ValidationResult("The document has no LineItems", currentTarget, key, "LineItems", this));
            }
            else
            {
                foreach (var item in objectToValidate.LineItems)
                {
                    lineItemValidator.Validate(item, validationResults);
                }
            }

            if (objectToValidate.Total.HasValue)
            {
                if (objectToValidate.Total.Value != objectToValidate.LineItems.Sum(a => a.GetTotal()))
                {
                    validationResults.AddResult(new ValidationResult("The document total does not equal the sum of the lines.", currentTarget, key, "Total", this));
                }
            }

            if (objectToValidate.TotalTax.HasValue)
            {
                if (objectToValidate.TotalTax.Value != objectToValidate.LineItems.Sum(a => a.TaxAmount))
                {
                    validationResults.AddResult(new ValidationResult("The document totaltax does not equal the sum of the lines.", currentTarget, key, "TotalTax", this));
                }
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { throw new NotImplementedException(); }
        }
    }
}
