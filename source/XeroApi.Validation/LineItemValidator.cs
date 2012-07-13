using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeroApi.Model;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using XeroApi.Validation.Helpers;

namespace XeroApi.Validation
{
    public class LineItemValidator : Validator<LineItem>
    {
        public LineItemValidator()
            : base(null, null)
        { }

        protected override void DoValidate(LineItem objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate.AccountCode.IsNullOrWhiteSpace())
            {
                validationResults.AddResult(new ValidationResult("No AccountCode Specified", currentTarget, key, "AccountCode", this));
            }

            if (objectToValidate.Description.IsNullOrWhiteSpace())
            {
                validationResults.AddResult(new ValidationResult("No Description Specified", currentTarget, key, "AccountCode", this));
            }

            if (objectToValidate.LineAmount.HasValue)
            {
                if (objectToValidate.LineAmount < 0)
                    validationResults.AddResult(new ValidationResult("LineAmount must be greater than 0", currentTarget, key, "LineAmount", this));
            }

            if (objectToValidate.UnitAmount.HasValue)
            {
                if (!objectToValidate.Quantity.HasValue)
                {
                    validationResults.AddResult(new ValidationResult("Quantity must be specified if UnitAmount is specified", currentTarget, key, "Quantity", this));
                }
                else if (objectToValidate.LineAmount.HasValue)
                {
                    if (objectToValidate.UnitAmount * objectToValidate.Quantity != objectToValidate.LineAmount)
                    {
                        validationResults.AddResult(new ValidationResult("LineAmount must be equal to Quantity * UnitAmount", currentTarget, key, "LineAmount", this));
                    }
                }
            }

            if (objectToValidate.Quantity.HasValue)
            {
                if (!objectToValidate.UnitAmount.HasValue)
                {
                    validationResults.AddResult(new ValidationResult("UnitAmount must be specified if Quantity is specified", currentTarget, key, "UnitAmount", this));
                }
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { return "The LineItem is invalid"; }
        }
    }
}
