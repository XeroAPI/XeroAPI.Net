using System;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;

using XeroApi.Model;
using XeroApi.Model.Payroll.Enums;
using XeroApi.Model.Serialize;
using XeroApi.Tests.Stubs;

namespace XeroApi.Tests
{
    [TestFixture]
    public class ApiQueryTests
    {
        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithNoArguments()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var organisations = repository.Organisations.ToList();

            Assert.AreEqual("Organisation", integrationProxy.LastQueryDescription.ElementName);
            Assert.AreEqual("", integrationProxy.LastQueryDescription.Order);
            Assert.AreEqual("", integrationProxy.LastQueryDescription.Where);
        }


        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithOneWhereArgument()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var organisations = repository.Organisations.Where(o => o.Name == "Demo Company (NZ)").ToList();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDescription.ElementType.Name);
            Assert.AreEqual("(Name == \"Demo Company (NZ)\")", queryDescription.Where);
            Assert.AreEqual("", queryDescription.Order);
        }

        [Test]
        public void TestApiQueryCanCallInvoicesEndpointWithContactNumberFilter()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var invoices = repository.Invoices.Where(inv => inv.Contact.ContactNumber == "S0029").ToList();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Invoice", queryDescription.ElementType.Name);
            Assert.AreEqual("(Contact.ContactNumber == \"S0029\")", queryDescription.Where);
            Assert.AreEqual("", queryDescription.Order);
        }

        [Test]
        public void it_can_parse_explicit_boolean_where_filter()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var users = repository.Users.Where(user => user.IsSubscriber).ToList();

            Assert.AreEqual("(IsSubscriber == true)", integrationProxy.LastQueryDescription.Where);
        }

        [Test]
        public void it_can_parse_implicit_boolean_where_filter()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var users = repository.Users.Where(user => user.IsSubscriber).ToList();

            Assert.AreEqual("(IsSubscriber == true)", integrationProxy.LastQueryDescription.Where);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithTwoWhereArguments()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Organisations
                .Where(o => o.Name == "Demo Company")
                .Where(o => o.APIKey == "ABCDEFG")
                .ToList();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDescription.ElementType.Name);
            Assert.AreEqual("(Name == \"Demo Company\") AND (APIKey == \"ABCDEFG\")", queryDescription.Where);
            Assert.AreEqual("", queryDescription.Order);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithFirstMethod()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Organisations.First();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDescription.ElementType.Name);
            Assert.AreEqual("", queryDescription.Where);
            Assert.AreEqual("", queryDescription.Order);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithFirstMethodWithPredicate()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Organisations.First(o => o.Name == "Demo Company");

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDescription.ElementType.Name);
            Assert.AreEqual("(Name == \"Demo Company\")", queryDescription.Where);
            Assert.AreEqual("", queryDescription.Order);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithFirstOrDefaultMethod()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            Organisation organisation = repository.Organisations.FirstOrDefault();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDescription.ElementType.Name);
            Assert.AreEqual("", queryDescription.Where);
            Assert.AreEqual("", queryDescription.Order);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithFirstOrDefaultMethodWithPredicate()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            Organisation organisation = repository.Organisations.FirstOrDefault(o => o.Name == "Demo Company");

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDescription.ElementType.Name);
            Assert.AreEqual("(Name == \"Demo Company\")", queryDescription.Where);
            Assert.AreEqual("", queryDescription.Order);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithCountMethod()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            int count = repository.Organisations.Count();
            
            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDescription.ElementType.Name);
            Assert.AreEqual("", queryDescription.Where);
            Assert.AreEqual("", queryDescription.Order);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithOneOrderByMethod()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Organisations.OrderBy(organisation => organisation.CreatedDateUTC).ToList();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDescription.ElementType.Name);
            Assert.AreEqual("", queryDescription.Where);
            Assert.AreEqual("CreatedDateUTC", queryDescription.Order);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithTwoOrderByMethods()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Organisations
                .OrderBy(organisation => organisation.CreatedDateUTC)
                .ThenBy(organisation => organisation.APIKey)
                .ToList();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDescription.ElementType.Name);
            Assert.AreEqual("", queryDescription.Where);
            Assert.AreEqual("CreatedDateUTC, APIKey", queryDescription.Order);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithOrderByDescMethod()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Organisations.OrderByDescending(organisation => organisation.CreatedDateUTC).ToList();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDescription.ElementType.Name);
            Assert.AreEqual("", queryDescription.Where);
            Assert.AreEqual("CreatedDateUTC DESC", queryDescription.Order);
        }

        [Test]
        public void TestApiQueryCanCallOrganisationsEndpointWithOrderByAndWhereMethod()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Organisations
                .Where(organisation => organisation.Name == "Demo Company")
                .OrderBy(organisation => organisation.CreatedDateUTC)
                .ToList();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Organisation", queryDescription.ElementType.Name);
            Assert.AreEqual("(Name == \"Demo Company\")", queryDescription.Where);
            Assert.AreEqual("CreatedDateUTC", queryDescription.Order);
        }
        
        [Test]
        public void TestApiQueryCanCallInvoicesEndpointFilteringByGuidParameter()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            Guid brandingThemeID = new Guid("071509D6-BADC-4237-9F52-AD2B4CCD9264");

            var response = repository.Invoices
                .Where(invoice => invoice.BrandingThemeID == brandingThemeID)
                .ToList();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Invoice", queryDescription.ElementType.Name);
            Assert.AreEqual("(BrandingThemeID == Guid(\"071509d6-badc-4237-9f52-ad2b4ccd9264\"))", queryDescription.Where);
        }


        [Test]
        public void TestApiQueryCanCallInvoicesEndpointFilteringByGuidConstant()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            Guid brandingThemeID = new Guid("071509D6-BADC-4237-9F52-AD2B4CCD9264");

            var response = repository.Invoices
                .Where(invoice => invoice.BrandingThemeID == brandingThemeID)
                .ToList();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Invoice", queryDescription.ElementType.Name);
            Assert.AreEqual("(BrandingThemeID == Guid(\"071509d6-badc-4237-9f52-ad2b4ccd9264\"))", queryDescription.Where);
        }

        [Test]
        public void TestApiQueryCanCallInvoicesEndpointFilteringByContactID()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Invoices
                .Where(invoice => invoice.Contact.ContactID == new Guid("071509D6-BADC-4237-9F52-AD2B4CCD9264"))
                .ToList();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Invoice", queryDescription.ElementType.Name);
            Assert.AreEqual("(Contact.ContactID == Guid(\"071509d6-badc-4237-9f52-ad2b4ccd9264\"))", queryDescription.Where);
        }
       

        [Test]
        public void it_can_filter_properties_on_object_properties_not_in_the_linq_definition()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var invoice = new Invoice {Contact = new Contact { Name = "Joe Bloggs" }};

            var response = repository.Invoices
                .Where(i => i.Contact.Name == invoice.Contact.Name)
                .ToList();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Invoice", queryDescription.ElementType.Name);
            Assert.AreEqual("(Contact.Name == \"Joe Bloggs\")", queryDescription.Where);
        }
        
        [Test]
        public void it_can_filter_on_collections_within_the_linq_definition()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var contact = new Contact { Addresses = new Addresses { new Address { City = "Moscow" } } };

            var response = repository.Invoices
                .Where(i => i.Contact.Addresses[0].City == contact.Addresses[0].City)
                .ToList();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Invoice", queryDescription.ElementType.Name);
            Assert.AreEqual("(Contact.Addresses[0].City == \"Moscow\")", queryDescription.Where);
        }

        [Test]
        public void TestApiQueryCanCallContactsEndpointWithAndAlsoOperator()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Contacts
                .Where(c => c.ContactStatus == "ACTIVE" && c.IsCustomer == true)
                .ToList();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Contact", queryDescription.ElementType.Name);
            Assert.AreEqual("((ContactStatus == \"ACTIVE\") AND (IsCustomer == true))", queryDescription.Where);
        }

        [Test]
        public void TestApiQueryCanCallContactsEndpointWithLinqAndAlsoOperator()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            IQueryable<Contact> query = from contact in repository.Contacts
                      where contact.ContactStatus == "ACTIVE" && contact.IsCustomer == true
                      select contact;

            var response = query.GetEnumerator();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("Contact", queryDescription.ElementType.Name);
            Assert.AreEqual("((ContactStatus == \"ACTIVE\") AND (IsCustomer == true))", queryDescription.Where);
        }

        [Test]
        public void TestApiQueryCanCallContactsEndpointWithOrElseOperator()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Contacts
                .Where(c => c.ContactStatus == "ACTIVE" || c.IsCustomer == true)
                .ToList();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("((ContactStatus == \"ACTIVE\") OR (IsCustomer == true))", queryDescription.Where);
        }

        [Test]
        public void TestApiQueryCanCallContactsEndpointWithOrElseAndAndAlsoOperator()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Contacts
                .Where(c => (c.ContactStatus == "ACTIVE" || c.IsCustomer == true) && (c.ContactStatus == "ARCHIVED"))
                .ToList();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("(((ContactStatus == \"ACTIVE\") OR (IsCustomer == true)) AND (ContactStatus == \"ARCHIVED\"))", queryDescription.Where);
        }
        
        [Test]
        public void TestApiQueryCanCallEmployeeEndpointWithImplicitBooleanAndNotOperator()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Users
                .Where(u => !u.IsSubscriber)
                .ToList();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("User", queryDescription.ElementType.Name);
            Assert.AreEqual("(IsSubscriber == false)", queryDescription.Where);
        }

        [Test]
        public void TestApiQueryCanCallEmployeeEndpointWithImplicitLinqBooleanOperator()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var query = from user in repository.Users
                        where user.IsSubscriber == false
                        select user;

            var response = query.GetEnumerator();

            var queryDescription = integrationProxy.LastQueryDescription;
            Assert.AreEqual("User", queryDescription.ElementType.Name);
            Assert.AreEqual("(IsSubscriber == false)", queryDescription.Where);
        }


        [Test]
        public void TestApiQueryCanCallEmployeeEndpointWithSingleOperator()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Users.Single(u => u.FullName == "Joe Bloggs");
            
            var queryDescription = integrationProxy.LastQueryDescription;

            Assert.AreEqual("User", queryDescription.ElementType.Name);
            Assert.AreEqual("Single", queryDescription.ClientSideExpression);
            Assert.AreEqual("(FullName == \"Joe Bloggs\")", queryDescription.Where);
        }

        [Test]
        public void TestApiQueryCanCallInvoicesEndpointWithUpdatedDateAndUrlFilterCombinedWithAndOperator()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Invoices.SingleOrDefault(i => i.UpdatedDateUTC > new DateTime(2010, 1, 1) && i.Url != null);
            
            var queryDescription = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Invoice", queryDescription.ElementType.Name);
            Assert.AreEqual("SingleOrDefault", queryDescription.ClientSideExpression);
            Assert.AreEqual("(Url <> NULL)", queryDescription.Where);
            Assert.AreEqual(new DateTime(2010, 01, 01), queryDescription.UpdatedSinceDate);
        }


        // Test for https://github.com/XeroAPI/XeroAPI.Net/issues/14
        [Test]
        public void TestApiQueryCanCallInvoicesEndpointWithUpdatedDateAndTypeFilterCombinedWithAndOperator()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Invoices.Where(i => i.UpdatedDateUTC > new DateTime(2010, 1, 1) && i.Type == "ACCPAY").ToList();

            var queryDescription = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Invoice", queryDescription.ElementType.Name);
            Assert.AreEqual(null, queryDescription.ClientSideExpression);
            Assert.AreEqual("(Type == \"ACCPAY\")", queryDescription.Where);
            Assert.AreEqual(new DateTime(2010, 01, 01), queryDescription.UpdatedSinceDate);
        }

        [Test]
        public void TestApiQueryCanCallContactsEndpointWithContactNumberIsNull()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Contacts.SingleOrDefault(i => i.ContactNumber == null);

            var queryDescription = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", queryDescription.ElementType.Name);
            Assert.AreEqual("SingleOrDefault", queryDescription.ClientSideExpression);
            Assert.AreEqual("(ContactNumber == NULL)", queryDescription.Where);
        }

        [Test]
        public void TestApiQueryCanCallContactsEndpointWithContactNumberIsNotNull()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Contacts.SingleOrDefault(i => i.ContactNumber != null);

            var queryDescription = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", queryDescription.ElementType.Name);
            Assert.AreEqual("SingleOrDefault", queryDescription.ClientSideExpression);
            Assert.AreEqual("(ContactNumber <> NULL)", queryDescription.Where);
        }

        [Test]
        public void it_can_filter_string_properties_on_methos_of_literal()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            repository.Contacts.FirstOrDefault(c => c.Name == "Jason".ToUpper());
            var queryDescription = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", queryDescription.ElementType.Name);
            Assert.AreEqual("(Name == \"JASON\")", queryDescription.Where);
        }

        [Test]
        public void it_can_filter_string_properties_using_contains_method()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            repository.Contacts.FirstOrDefault(c => c.Name.Contains("Coffee"));
            var queryDescription = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", queryDescription.ElementType.Name);
            Assert.AreEqual("Name.Contains(\"Coffee\")", queryDescription.Where);
        }

        [Test]
        public void it_can_filter_string_properties_using_startswith_method()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            repository.Contacts.FirstOrDefault(c => c.Name.StartsWith("Coffee"));

            var queryDescription = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", queryDescription.ElementType.Name);
            Assert.AreEqual("Name.StartsWith(\"Coffee\")", queryDescription.Where);
        }

        [Test]
        public void it_can_filter_string_properties_using_startswith_method_with_another_filter()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Contacts.FirstOrDefault(c => c.Name.StartsWith("Coffee") && c.IsCustomer == true);

            var queryDescription = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", queryDescription.ElementType.Name);
            Assert.AreEqual("(Name.StartsWith(\"Coffee\") AND (IsCustomer == true))", queryDescription.Where);
        }

        [Test]
        public void it_does_not_implement_contains_linq_method()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            Assert.Throws<NotImplementedException>(() => repository.Contacts.Contains(new Contact { Name = "Jason" }));
        }

        [Test]
        public void it_can_filter_string_properties_using_static_methods()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Contacts.Where(c => string.IsNullOrEmpty(c.ContactNumber)).ToList();

            var queryDescription = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", queryDescription.ElementType.Name);
            Assert.AreEqual("String.IsNullOrEmpty(ContactNumber)", queryDescription.Where);
        }

        [Test]
        public void it_can_filter_string_properties_using_static_methods_with_implicit_negation()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Contacts.Where(c => !string.IsNullOrEmpty(c.ContactNumber)).ToList();

            var lastQueryDescription = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", lastQueryDescription.ElementType.Name);
            Assert.AreEqual("(String.IsNullOrEmpty(ContactNumber) == false)", lastQueryDescription.Where);
        }

        [Test]
        public void it_can_filter_string_properties_using_static_methods_with_implicit_affirmation()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Contacts.Where(c => string.IsNullOrEmpty(c.ContactNumber)).ToList();

            var lastQueryDescription = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Contact", lastQueryDescription.ElementType.Name);
            Assert.AreEqual("String.IsNullOrEmpty(ContactNumber)", lastQueryDescription.Where);
        }

        [Test]
        public void it_can_filter_on_a_nullable_date_property_with_an_inline_value()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            var response = repository.Invoices.Where(i => i.Date == new DateTime(2012, 01, 04)).ToList();

            var lastQueryDescription = integrationProxy.LastQueryDescription;

            Assert.AreEqual("Invoice", lastQueryDescription.ElementType.Name);
            Assert.AreEqual("(Date == DateTime(2012,1,4))", lastQueryDescription.Where);
        }

        [Test]
        public void it_can_filter_using_a_predefined_predicate_expression_variable()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            Expression<Func<Invoice, bool>> filterInvoiceByCurrency = i => i.CurrencyCode == "AUD";

            var response = repository.Invoices.Where(filterInvoiceByCurrency).ToList();
            
            Assert.AreEqual("(CurrencyCode == \"AUD\")", integrationProxy.LastQueryDescription.Where);
        }

        [Test]
        public void it_can_filter_using_member_method_with_no_arguments()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());
            
            repository.Invoices.Where(i => i.InvoiceNumber.ToLower() == "inv-123").ToList();

            Assert.AreEqual("(InvoiceNumber.ToLower() == \"inv-123\")", integrationProxy.LastQueryDescription.Where);
        }

        [Ignore("This is a known defect")]
        [Test]
        public void it_can_filter_on_inequality_of_the_model_id()
        {
            var integrationProxy = GetProxy();
            var repository = new CoreRepository(integrationProxy, GetSerializer());

            repository.Invoices.Where(i => i.InvoiceID != Guid.Empty).ToList();

            Assert.AreEqual("", integrationProxy.LastQueryDescription.ElementId);
            Assert.AreEqual("(InvoiceID != Guid('00000000-0000-0000-0000-000000000000'))", integrationProxy.LastQueryDescription.Where);
        }

        [Test]
        public void it_can_filter_using_enum_values_equal_to()
        {
            var integrationProxy = GetProxy();
            
            var repository = new PayrollRepository(integrationProxy, GetSerializer());

            repository.PayrollCalendars.Where(it => it.CalendarType == CalendarType.MONTHLY).ToList();

            Assert.AreEqual("(CalendarType == \"MONTHLY\")", integrationProxy.LastQueryDescription.Where);
        }

        [Test]
        public void it_can_filter_using_enum_values_not_equal_to()
        {
            var integrationProxy = GetProxy();

            var repository = new PayrollRepository(integrationProxy, GetSerializer());

            repository.PayrollCalendars.Where(it => it.CalendarType != CalendarType.MONTHLY).ToList();

            Assert.AreEqual("(CalendarType <> \"MONTHLY\")", integrationProxy.LastQueryDescription.Where);
        }

        private static bool IsXml
        {
            get { return true; }
        }
    
        private static IModelSerializer GetSerializer()
        {
            return IsXml ? new XmlModelSerializer() as IModelSerializer : new JsonModelSerializer();
        }

        private static StubIntegrationProxyBase GetProxy()
        {
            return IsXml ? new XmlStubIntegrationProxy() as StubIntegrationProxyBase : new JsonStubIntegrationProxy();
        }
    }
}

