using System;
using System.Collections.Specialized;
using NUnit.Framework;
using XeroApi.Integration;

namespace XeroApi.Tests
{
    [TestFixture]
    public class IntegrationProxyHelperTests
    {
        [Test]
        public void TestIntegrationProxyHelperCanGenerateSimpleUri()
        {
            Uri uri = IntegrationProxy.ConstructUri(
                new Uri("https://api.xero.com/api.xro/2.0"),
                "Invoices",
                "",
                null);

            Assert.AreEqual("https://api.xero.com/api.xro/2.0/Invoices", uri.ToString());
        }

        [Test]
        public void TestIntegrationProxyHelperCanGenerateSimpleUri2()
        {
            Uri uri = IntegrationProxy.ConstructUri(
                new Uri("https://api.xero.com/api.xro/2.0"),
                "Invoices",
                "INV-123",
                null);

            Assert.AreEqual("https://api.xero.com/api.xro/2.0/Invoices/INV-123", uri.ToString());
        }

        [Test]
        public void TestIntegrationProxyHelperCanGenerateSimpleUri3()
        {
            Uri uri = IntegrationProxy.ConstructUri(
                new Uri("https://api.xero.com/api.xro/2.0"),
                "Invoices",
                "INV-123",
                new NameValueCollection { { "WHERE", "(foo = bar)" } });

            Assert.AreEqual("https://api.xero.com/api.xro/2.0/Invoices/INV-123?WHERE=%28foo%20%3D%20bar%29", uri.AbsoluteUri);
        }

        [Test]
        public void TestIntegrationProxyHelperCanGenerateSimpleUri4()
        {
            Uri uri = IntegrationProxy.ConstructUri(
                new Uri("https://api.xero.com/api.xro/2.0"),
                "Invoices",
                "INV-123",
                new NameValueCollection { { "WHERE", "(foo = bar)" }, { "ORDER", "InvoiceNumber DESC" } });

            Assert.AreEqual("https://api.xero.com/api.xro/2.0/Invoices/INV-123?WHERE=%28foo%20%3D%20bar%29&ORDER=InvoiceNumber%20DESC", uri.AbsoluteUri);
        }

        [Test]
        public void TestIntegrationProxyHelperCanGenerateSimpleUri5()
        {
            Uri uri = IntegrationProxy.ConstructUri(
                new Uri("https://api.xero.com/api.xro/2.0"),
                "Invoices",
                "",
                new NameValueCollection { { "WHERE", "(foo = bar)" }, { "ORDER", "InvoiceNumber DESC" } });

            Assert.AreEqual("https://api.xero.com/api.xro/2.0/Invoices?WHERE=%28foo%20%3D%20bar%29&ORDER=InvoiceNumber%20DESC", uri.AbsoluteUri);
        }
    }
}
