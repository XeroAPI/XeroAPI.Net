using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using XeroApi.Model;
using XeroApi.Tests.Stubs;

namespace XeroApi.Tests
{
    [TestFixture]
    public class ApiQueryTests
    {

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithNoArguments()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            List<Organisation> organisations = repository.Organisations.ToList();

            Assert.AreEqual(0, organisations.Count);
            Assert.AreEqual("Organisation", integrationProxy.LastQueryDescription.ElementName);
        }


        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithOneWhereArgument()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            List<Organisation> organisations = repository.Organisations.Where(o => o.Name == "Demo Company (NZ)").ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(Name == \"Demo Company (NZ)\")", queryDesctipion.Where);
            Assert.AreEqual("", queryDesctipion.Order);
            Assert.AreEqual(0, organisations.Count);
        }

        [Test]
        public void TestApiQueryCanCallInvoicesEndpointWithContactNumberFilter()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            List<Invoice> invoices = repository.Invoices.Where(inv => inv.Contact.ContactNumber == "S0029").ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Invoice", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(Contact.ContactNumber == \"S0029\")", queryDesctipion.Where);
            Assert.AreEqual("", queryDesctipion.Order);
            Assert.AreEqual(0, invoices.Count);
        }

        [Test]
        public void TestApiQueryCanCallUsersEndpointWithBooleanFilter()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            // Note: This needs the '== true' but to work correctly
            List<User> invoices = repository.Users.Where(user => user.IsSubscriber == true).ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("User", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(IsSubscriber == true)", queryDesctipion.Where);
            Assert.AreEqual("", queryDesctipion.Order);
            Assert.AreEqual(0, invoices.Count);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithTwoWhereArguments()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            repository.Organisations
                .Where(o => o.Name == "Demo Company")
                .Where(o => o.APIKey == "ABCDEFG")
                .ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(Name == \"Demo Company\") AND (APIKey == \"ABCDEFG\")", queryDesctipion.Where);
            Assert.AreEqual("", queryDesctipion.Order);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithFirstMethod()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            Assert.Throws<InvalidOperationException>(() => repository.Organisations.First());

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDesctipion.ElementType.Name);
            Assert.AreEqual("", queryDesctipion.Where);
            Assert.AreEqual("", queryDesctipion.Order);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithFirstMethodWithPredicate()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            Assert.Throws<InvalidOperationException>(() => repository.Organisations.First(o => o.Name == "Demo Company"));

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(Name == \"Demo Company\")", queryDesctipion.Where);
            Assert.AreEqual("", queryDesctipion.Order);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithFirstOrDefaultMethod()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            Organisation organisation = repository.Organisations.FirstOrDefault();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDesctipion.ElementType.Name);
            Assert.AreEqual("", queryDesctipion.Where);
            Assert.AreEqual("", queryDesctipion.Order);
            Assert.IsNull(organisation);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithFirstOrDefaultMethodWithPredicate()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            Organisation organisation = repository.Organisations.FirstOrDefault(o => o.Name == "Demo Company");

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(Name == \"Demo Company\")", queryDesctipion.Where);
            Assert.AreEqual("", queryDesctipion.Order);
            Assert.IsNull(organisation);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithCountMethod()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            int organisationCount = repository.Organisations.Count();
            
            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDesctipion.ElementType.Name);
            Assert.AreEqual("", queryDesctipion.Where);
            Assert.AreEqual("", queryDesctipion.Order);
            Assert.AreEqual(0, organisationCount);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithOneOrderByMethod()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            repository.Organisations.OrderBy(organisation => organisation.CreatedDateUTC).ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDesctipion.ElementType.Name);
            Assert.AreEqual("", queryDesctipion.Where);
            Assert.AreEqual("CreatedDateUTC", queryDesctipion.Order);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithTwoOrderByMethods()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            repository.Organisations
                .OrderBy(organisation => organisation.CreatedDateUTC)
                .OrderBy(organisation => organisation.APIKey)
                .ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDesctipion.ElementType.Name);
            Assert.AreEqual("", queryDesctipion.Where);
            Assert.AreEqual("CreatedDateUTC, APIKey", queryDesctipion.Order);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithOrderByDescMethod()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            repository.Organisations.OrderByDescending(organisation => organisation.CreatedDateUTC).ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDesctipion.ElementType.Name);
            Assert.AreEqual("", queryDesctipion.Where);
            Assert.AreEqual("CreatedDateUTC DESC", queryDesctipion.Order);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithOrderByAndWhereMethod()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            repository.Organisations
                .Where(organisation => organisation.Name == "Demo Company")
                .OrderBy(organisation => organisation.CreatedDateUTC)
                .ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(Name == \"Demo Company\")", queryDesctipion.Where);
            Assert.AreEqual("CreatedDateUTC", queryDesctipion.Order);
        }
        
        [Test]
        public void TestApiQueryCanCallInvoicesEndpointFilteringByGuidParameter()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            Guid brandingThemeID = new Guid("071509D6-BADC-4237-9F52-AD2B4CCD9264");

            repository.Invoices
                .Where(invoice => invoice.BrandingThemeID == brandingThemeID)
                .ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Invoice", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(BrandingThemeID == Guid(\"071509d6-badc-4237-9f52-ad2b4ccd9264\"))", queryDesctipion.Where);
        }


        [Test]
        public void TestApiQueryCanCallInvoicesEndpointFilteringByGuidConstant()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            Guid brandingThemeID = new Guid("071509D6-BADC-4237-9F52-AD2B4CCD9264");

            repository.Invoices
                .Where(invoice => invoice.BrandingThemeID == brandingThemeID)
                .ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Invoice", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(BrandingThemeID == Guid(\"071509d6-badc-4237-9f52-ad2b4ccd9264\"))", queryDesctipion.Where);
        }

        [Test]
        public void TestApiQueryCanCallInvoicesEndpointFilteringByContactID()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            repository.Invoices
                .Where(invoice => invoice.Contact.ContactID == new Guid("071509D6-BADC-4237-9F52-AD2B4CCD9264"))
                .ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Invoice", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(Contact.ContactID == Guid(\"071509d6-badc-4237-9f52-ad2b4ccd9264\"))", queryDesctipion.Where);
        }
    }
}

