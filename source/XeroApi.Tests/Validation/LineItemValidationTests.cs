using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using XeroApi.Model;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using XeroApi.Validation;

namespace XeroApi.Tests.Validation
{
    public class LineItemValidationTests
    {
        ValidatorFactory valFactory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();

        [Test]
        public void lineitem_null_description_fails()
        {
            Validator<LineItem> liValidator = new LineItemValidator();
            LineItem li = new LineItem();
            var results = liValidator.Validate(li);
            Assert.Greater(results.Count, 0);
        }
    }
}
