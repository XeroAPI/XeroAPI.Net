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
        static IUnityContainer container = new UnityContainer();

        static ValidationHelper()
        {
            container.RegisterType(typeof(Validator<Invoice>), typeof(InvoiceValidator));
            container.RegisterType(typeof(Validator<LineItem>), typeof(LineItemValidator));
        }

        public static ValidationResults Validate<T>(this T i) where T : ModelBase
        {
            var val = container.Resolve<Validator<T>>();
            var retVal = val.Validate(i);
            return retVal;
        }

        public static bool IsValid<T>(this T i) where T : ModelBase
        {
            return !i.Validate().Any();
        }
    }
}
