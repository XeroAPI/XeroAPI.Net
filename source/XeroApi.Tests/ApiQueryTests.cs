﻿using System;
using System.Linq;
using System.Linq.Expressions;
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

            var organisations = repository.Organisations.ToList();

            Assert.AreEqual("Organisation", integrationProxy.LastQueryDescription.ElementName);
            Assert.AreEqual("", integrationProxy.LastQueryDescription.Order);
            Assert.AreEqual("", integrationProxy.LastQueryDescription.Where);
        }


        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithOneWhereArgument()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var organisations = repository.Organisations.Where(o => o.Name == "Demo Company (NZ)").ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(Name == \"Demo Company (NZ)\")", queryDesctipion.Where);
            Assert.AreEqual("", queryDesctipion.Order);
        }

        [Test]
        public void TestApiQueryCanCallInvoicesEndpointWithContactNumberFilter()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var invoices = repository.Invoices.Where(inv => inv.Contact.ContactNumber == "S0029").ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Invoice", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(Contact.ContactNumber == \"S0029\")", queryDesctipion.Where);
            Assert.AreEqual("", queryDesctipion.Order);
        }

        [Test]
        public void it_can_parse_explicit_boolean_where_filter()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var users = repository.Users.Where(user => user.IsSubscriber == true).ToList();

            Assert.AreEqual("(IsSubscriber == true)", integrationProxy.LastQueryDescription.Where);
        }

        [Test]
        public void it_can_parse_implicit_boolean_where_filter()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var users = repository.Users.Where(user => user.IsSubscriber).ToList();

            Assert.AreEqual("(IsSubscriber == true)", integrationProxy.LastQueryDescription.Where);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithTwoWhereArguments()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Organisations
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

            var response = repository.Organisations.First();

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

            var response = repository.Organisations.First(o => o.Name == "Demo Company");

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
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithCountMethod()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            int count = repository.Organisations.Count();
            
            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDesctipion.ElementType.Name);
            Assert.AreEqual("", queryDesctipion.Where);
            Assert.AreEqual("", queryDesctipion.Order);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithOneOrderByMethod()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Organisations.OrderBy(organisation => organisation.CreatedDateUTC).ToList();

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

            var response = repository.Organisations
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

            var response = repository.Organisations.OrderByDescending(organisation => organisation.CreatedDateUTC).ToList();

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

            var response = repository.Organisations
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

            var response = repository.Invoices
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

            var response = repository.Invoices
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

            var response = repository.Invoices
                .Where(invoice => invoice.Contact.ContactID == new Guid("071509D6-BADC-4237-9F52-AD2B4CCD9264"))
                .ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Invoice", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(Contact.ContactID == Guid(\"071509d6-badc-4237-9f52-ad2b4ccd9264\"))", queryDesctipion.Where);
        }
       

        [Test]
        public void it_can_filter_properties_on_object_properties_not_in_the_linq_definition()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var invoice = new Invoice {Contact = new Contact { Name = "Joe Bloggs" }};

            var response = repository.Invoices
                .Where(i => i.Contact.Name == invoice.Contact.Name)
                .ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Invoice", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(Contact.Name == \"Joe Bloggs\")", queryDesctipion.Where);
        }
        
        [Test]
        public void it_can_filter_on_collections_within_the_linq_definition()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var contact = new Contact { Addresses = new Addresses { new Address { City = "Moscow" } } };

            var response = repository.Invoices
                .Where(i => i.Contact.Addresses[0].City == contact.Addresses[0].City)
                .ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Invoice", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(Contact.Addresses[0].City == \"Moscow\")", queryDesctipion.Where);
        }

        [Test]
        public void TestApiQueryCanCallContactsEndpointWithAndAlsoOperator()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Contacts
                .Where(c => c.ContactStatus == "ACTIVE" && c.IsCustomer == true)
                .ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Contact", queryDesctipion.ElementType.Name);
            Assert.AreEqual("((ContactStatus == \"ACTIVE\") AND (IsCustomer == true))", queryDesctipion.Where);
        }

        [Test]
        public void TestApiQueryCanCallContactsEndpointWithLinqAndAlsoOperator()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            IQueryable<Contact> query = from contact in repository.Contacts
                      where contact.ContactStatus == "ACTIVE" && contact.IsCustomer == true
                      select contact;

            var response = query.GetEnumerator();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Contact", queryDesctipion.ElementType.Name);
            Assert.AreEqual("((ContactStatus == \"ACTIVE\") AND (IsCustomer == true))", queryDesctipion.Where);
        }

        [Test]
        public void TestApiQueryCanCallContactsEndpointWithOrElseOperator()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Contacts
                .Where(c => c.ContactStatus == "ACTIVE" || c.IsCustomer == true)
                .ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("((ContactStatus == \"ACTIVE\") OR (IsCustomer == true))", queryDesctipion.Where);
        }

        [Test]
        public void TestApiQueryCanCallContactsEndpointWithOrElseAndAndAlsoOperator()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Contacts
                .Where(c => (c.ContactStatus == "ACTIVE" || c.IsCustomer == true) && (c.ContactStatus == "ARCHIVED"))
                .ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("(((ContactStatus == \"ACTIVE\") OR (IsCustomer == true)) AND (ContactStatus == \"ARCHIVED\"))", queryDesctipion.Where);
        }
        
        [Test]
        public void TestApiQueryCanCallEmployeeEndpointWithImplicitBooleanAndNotOperator()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Users
                .Where(u => !u.IsSubscriber)
                .ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("User", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(IsSubscriber == false)", queryDesctipion.Where);
        }

        [Test]
        public void TestApiQueryCanCallEmployeeEndpointWithImplicitLinqBooleanOperator()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var query = from user in repository.Users
                        where user.IsSubscriber == false
                        select user;

            var response = query.GetEnumerator();

            var queryDesctipion = integrationProxy.LastQueryDescription;
            Assert.AreEqual("User", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(IsSubscriber == false)", queryDesctipion.Where);
        }


        [Test]
        public void TestApiQueryCanCallEmployeeEndpointWithSingleOperator()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Users.Single(u => u.FullName == "Joe Bloggs");
            
            var queryDesctipion = integrationProxy.LastQueryDescription;

            Assert.AreEqual("User", queryDesctipion.ElementType.Name);
            Assert.AreEqual("Single", queryDesctipion.ClientSideExpression);
            Assert.AreEqual("(FullName == \"Joe Bloggs\")", queryDesctipion.Where);
        }

        [Test]
        public void TestApiQueryCanCallInvoicesEndpointWithUpdatedDateAndUrlFilterCombinedWithAndOperator()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Invoices.SingleOrDefault(i => i.UpdatedDateUTC > new DateTime(2010, 1, 1) && i.Url != null);
            
            var queryDesctipion = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Invoice", queryDesctipion.ElementType.Name);
            Assert.AreEqual("SingleOrDefault", queryDesctipion.ClientSideExpression);
            Assert.AreEqual("(Url <> NULL)", queryDesctipion.Where);
            Assert.AreEqual(new DateTime(2010, 01, 01), queryDesctipion.UpdatedSinceDate);
        }


        // Test for https://github.com/XeroAPI/XeroAPI.Net/issues/14
        [Test]
        public void TestApiQueryCanCallInvoicesEndpointWithUpdatedDateAndTypeFilterCombinedWithAndOperator()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Invoices.Where(i => i.UpdatedDateUTC > new DateTime(2010, 1, 1) && i.Type == "ACCPAY").ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Invoice", queryDesctipion.ElementType.Name);
            Assert.AreEqual(null, queryDesctipion.ClientSideExpression);
            Assert.AreEqual("(Type == \"ACCPAY\")", queryDesctipion.Where);
            Assert.AreEqual(new DateTime(2010, 01, 01), queryDesctipion.UpdatedSinceDate);
        }

        [Test]
        public void TestApiQueryCanCallContactsEndpointWithContactNumberIsNull()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Contacts.SingleOrDefault(i => i.ContactNumber == null);

            var queryDesctipion = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", queryDesctipion.ElementType.Name);
            Assert.AreEqual("SingleOrDefault", queryDesctipion.ClientSideExpression);
            Assert.AreEqual("(ContactNumber == NULL)", queryDesctipion.Where);
        }

        [Test]
        public void TestApiQueryCanCallContactsEndpointWithContactNumberIsNotNull()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Contacts.SingleOrDefault(i => i.ContactNumber != null);

            var queryDesctipion = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", queryDesctipion.ElementType.Name);
            Assert.AreEqual("SingleOrDefault", queryDesctipion.ClientSideExpression);
            Assert.AreEqual("(ContactNumber <> NULL)", queryDesctipion.Where);
        }

        [Test]
        public void it_can_filter_string_properties_on_methos_of_literal()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            repository.Contacts.FirstOrDefault(c => c.Name == "Jason".ToUpper());
            var queryDesctipion = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(Name == \"JASON\")", queryDesctipion.Where);
        }

        [Test]
        public void it_can_filter_string_properties_using_contains_method()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            repository.Contacts.FirstOrDefault(c => c.Name.Contains("Coffee"));
            var queryDesctipion = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", queryDesctipion.ElementType.Name);
            Assert.AreEqual("Name.Contains(\"Coffee\")", queryDesctipion.Where);
        }

        [Test]
        public void it_can_filter_string_properties_using_startswith_method()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Contacts.FirstOrDefault(c => c.Name.StartsWith("Coffee"));

            var queryDesctipion = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", queryDesctipion.ElementType.Name);
            Assert.AreEqual("Name.StartsWith(\"Coffee\")", queryDesctipion.Where);
        }

        [Test]
        public void it_can_filter_string_properties_using_startswith_method_with_another_filter()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Contacts.FirstOrDefault(c => c.Name.StartsWith("Coffee") && c.IsCustomer == true);

            var queryDesctipion = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(Name.StartsWith(\"Coffee\") AND (IsCustomer == true))", queryDesctipion.Where);
        }

        [Test]
        public void it_does_not_implement_contains_linq_method()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            Assert.Throws<NotImplementedException>(() => repository.Contacts.Contains(new Contact { Name = "Jason" }));
        }

        [Test]
        public void it_can_filter_string_properties_using_static_methods()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Contacts.Where(c => string.IsNullOrEmpty(c.ContactNumber)).ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", queryDesctipion.ElementType.Name);
            Assert.AreEqual("String.IsNullOrEmpty(ContactNumber)", queryDesctipion.Where);
        }

        [Test]
        public void it_can_filter_string_properties_using_static_methods_with_implicit_negation()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Contacts.Where(c => !string.IsNullOrEmpty(c.ContactNumber)).ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(String.IsNullOrEmpty(ContactNumber) == false)", queryDesctipion.Where);
        }

        [Test]
        public void it_can_filter_string_properties_using_static_methods_with_implicit_affirmation()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Contacts.Where(c => string.IsNullOrEmpty(c.ContactNumber)).ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", queryDesctipion.ElementType.Name);
            Assert.AreEqual("String.IsNullOrEmpty(ContactNumber)", queryDesctipion.Where);
        }

        [Test]
        public void it_can_filter_on_a_nullable_date_property_with_an_inline_value()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            var response = repository.Invoices.Where(i => i.Date == new DateTime(2012, 01, 04)).ToList();

            var queryDesctipion = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Invoice", queryDesctipion.ElementType.Name);
            Assert.AreEqual("(Date == DateTime(2012,1,4))", queryDesctipion.Where);
        }

        [Test]
        public void it_can_filter_using_a_predefined_predicate_expression_variable()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            Expression<Func<Invoice, bool>> filterInvoiceByCurrency = i => i.CurrencyCode == "AUD";

            var response = repository.Invoices.Where(filterInvoiceByCurrency).ToList();
            
            Assert.AreEqual("(CurrencyCode == \"AUD\")", integrationProxy.LastQueryDescription.Where);
        }

        [Test]
        public void it_can_filter_using_member_method_with_no_arguments()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);
            
            repository.Invoices.Where(i => i.InvoiceNumber.ToLower() == "inv-123").ToList();

            Assert.AreEqual("(InvoiceNumber.ToLower() == \"inv-123\")", integrationProxy.LastQueryDescription.Where);
        }

        [Ignore("This is a known defect")]
        [Test]
        public void it_can_filter_on_inequality_of_the_model_id()
        {
            StubIntegrationProxy integrationProxy = new StubIntegrationProxy();
            Repository repository = new Repository(integrationProxy);

            repository.Invoices.Where(i => i.InvoiceID != Guid.Empty).ToList();

            Assert.AreEqual("", integrationProxy.LastQueryDescription.ElementId);
            Assert.AreEqual("(InvoiceID != Guid('00000000-0000-0000-0000-000000000000'))", integrationProxy.LastQueryDescription.Where);
        }
    }
}

