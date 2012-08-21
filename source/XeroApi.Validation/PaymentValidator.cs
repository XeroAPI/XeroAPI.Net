using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeroApi.Model;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace XeroApi.Validation
{
    public class PaymentValidator : Validator<Payment>
    {
        public PaymentValidator()
            : base(null, null)
        {
        }

        protected override void DoValidate(Payment objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (objectToValidate.Amount < 0)
            {
                validationResults.AddResult(new ValidationResult("The document amount must be greater than 0.", currentTarget, key, "Amount", this));
            }

            if (string.IsNullOrEmpty(objectToValidate.Reference))
            {
                validationResults.AddResult(new ValidationResult("Document Reference must be specified.", currentTarget, key, "Reference", this));
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { throw new NotImplementedException(); }
        }
    }
}
