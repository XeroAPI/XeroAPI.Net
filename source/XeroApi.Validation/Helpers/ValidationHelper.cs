using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using XeroApi.Model;

namespace XeroApi.Validation.Helpers
{
    public static class ValidationHelper
    {
        static readonly IUnityContainer container = new UnityContainer();

        public static IUnityContainer Container { get { return container; } }

        static ValidationHelper()
        {
            container.RegisterType(typeof(Validator<Invoice>), typeof(InvoiceValidator));
            container.RegisterType(typeof(Validator<LineItem>), typeof(LineItemValidator));
            container.RegisterType(typeof(Validator<CreditNote>), typeof(CreditNoteValidator));
            container.RegisterType(typeof(Validator<BankTransaction>), typeof(BankTransactionValidator));
            container.RegisterType(typeof(Validator<Payment>), typeof(PaymentValidator));
            container.RegisterType(typeof(Validator<ManualJournal>), typeof(ManualJournalValidator));
            container.RegisterType(typeof(Validator<ManualJournalLineItem>), typeof(ManualJournalLineItemValidator));
        }

        public static ValidationResults Validate<T>(this T i) where T : EndpointModelBase
        {
            var val = container.Resolve<Validator<T>>();
            var retVal = val.Validate(i);
            return retVal;
        }

        public static ValidationResults ValidateMany<T>(this IEnumerable<T> i) where T : EndpointModelBase
        {
            ValidationResults vr = new ValidationResults();
            foreach (var item in i)
            {
                vr.AddAllResults(item.Validate());
            }
            return vr;
        }

        public static bool IsValid<T>(this T i) where T : EndpointModelBase
        {
            return !i.Validate().Any();
        }
    }
}
