using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XeroApi.Model;
using XeroApi.Validation.Helpers;
using Microsoft.Practices.Unity;

namespace XeroApi.Validation
{
    public class ManualJournalValidator : Validator<ManualJournal>
    {
        Validator<ManualJournalLineItem> lineItemValidator = null;

        public ManualJournalValidator(Validator<ManualJournalLineItem> lineItemValidator)
            : base(null, null)
        {
            this.lineItemValidator = lineItemValidator;
        }

        public ManualJournalValidator()
            : base(null, null)
        {
            this.lineItemValidator = ValidationHelper.Container.Resolve<Validator<ManualJournalLineItem>>();
        }

        protected override void DoValidate(ManualJournal objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            if (string.IsNullOrEmpty(objectToValidate.Narration))
            {
                validationResults.AddResult(new ValidationResult("Document Narration must be specified.", currentTarget, key, "Narration", this));            
            }

            if (objectToValidate.JournalLines == null || !objectToValidate.JournalLines.Any())
            {
                validationResults.AddResult(new ValidationResult("The document has no LineItems", currentTarget, key, "JournalLines", this));
            }
            else
            {
                ValidationResults vr = new ValidationResults();
                foreach (var item in objectToValidate.JournalLines)
                {
                    lineItemValidator.Validate(item, vr);
                }
                if (vr.Any())
                {
                    validationResults.AddResult(new ValidationResult("Invalid LineItems", currentTarget, key, "LineItems", this, vr));
                }
            }
        }

        protected override string DefaultMessageTemplate
        {
            get { throw new NotImplementedException(); }
        }
    }
}
