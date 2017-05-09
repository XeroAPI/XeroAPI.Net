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
            if (objectToValidate.Amount <= 0)
            {
                validationResults.AddResult(new ValidationResult("The document amount must be greater than 0.", currentTarget, key, "Amount", this));
            }

            if (objectToValidate.Invoice == null)
            {
                validationResults.AddResult(new ValidationResult("Invoice element must be included.", currentTarget, key, "Invoice", this));
            }
            else
            {
                if (objectToValidate.Invoice.InvoiceID == Guid.Empty && string.IsNullOrEmpty(objectToValidate.Invoice.InvoiceNumber))
                {
                    validationResults.AddResult(new ValidationResult("Either InvoiceID or InvoiceNumber must be specified.", currentTarget, key, "Invoice", this));
                }
            }

            if (objectToValidate.Account == null)
            {
                validationResults.AddResult(new ValidationResult("Account element must be included.", currentTarget, key, "Account", this));
            }
            else
            {
                if (objectToValidate.Account.AccountID == Guid.Empty && string.IsNullOrEmpty(objectToValidate.Account.Code))
                {
                    validationResults.AddResult(new ValidationResult("Either AccountID or Code must be specified.", currentTarget, key, "Account", this));
                }
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { throw new NotImplementedException(); }
        }
    }
}
