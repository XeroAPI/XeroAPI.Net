using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeroApi.Model;
using XeroApi.Validation.Helpers;

namespace XeroApi.Validation
{
    public class ManualJournalLineItemValidator : Validator<ManualJournalLineItem>
    {
        public ManualJournalLineItemValidator()
            : base(null, null)
        { }

        protected override void DoValidate(ManualJournalLineItem objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate.AccountCode.IsNullOrWhiteSpace())
            {
                validationResults.AddResult(new ValidationResult("No AccountCode Specified", currentTarget, key, "AccountCode", this));
            }

            if (objectToValidate.LineAmount.GetValueOrDefault() == 0)
            {
                validationResults.AddResult(new ValidationResult("LineAmount must be not equal to 0", currentTarget, key, "LineAmount", this));
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { throw new NotImplementedException(); }
        }
    }
}
