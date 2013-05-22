using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using DevDefined.OAuth.Consumer;
using NUnit.Framework;

using Rhino.Mocks;

namespace XeroApi.Tests.OAuth.Consumer
{
    [TestFixture]
    public class ConsumerRequestRunnerTests
    {
        [Test]
        public void it_should_throw_a_sensible_error_when_ssl_certificates_cannot_be_read()
        {
            var consumerRequestRunner = new DefaultConsumerRequestRunner();

            var mockConsumerRequest = MockRepository.GenerateMock<IConsumerRequest>();
            var mockWebRequest = MockRepository.GenerateMock<HttpWebRequest>();
            mockWebRequest.Expect(i => i.GetResponse())
                          .Throw(new WebException("The request was aborted: Could not create SSL/TLS secure channel."));

            //I tried to mock an X509Certificate2, but rhino wouldn't mock the Subject property or GetNameInfo function without 
            // first running it, and thereby throwing an exception
            var mockCertificate = MockRepository.GenerateMock<X509Certificate>();
            mockCertificate.Stub(i => i.GetSerialNumberString()).Return("serial");

            mockWebRequest.Expect(i => i.ClientCertificates)
                          .Return(new X509CertificateCollection(new[] { mockCertificate }));

            mockConsumerRequest.Expect(i => i.ToWebRequest()).Return(mockWebRequest);

            try
            {
                consumerRequestRunner.Run(mockConsumerRequest);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<WebException>(ex);
                Assert.IsTrue(ex.Message.Contains("has permission to read the following certificates"));
            }
        }
    }
}
