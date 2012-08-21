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
            throw new NotImplementedException();
        }

        protected override string DefaultMessageTemplate
        {
            get { throw new NotImplementedException(); }
        }
    }
}
