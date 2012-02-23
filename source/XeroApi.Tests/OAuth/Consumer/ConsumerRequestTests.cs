using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace XeroApi.Tests.OAuth.Consumer {
    
    [TestFixture]
    public class ConsumerRequestTests
    {

        private IOAuthSession _oAuthSession;
        private IOAuthContext _oAuthContext;
        private NameValueCollection _headers;
        private ConsumerRequest _consumerRequest;

        [SetUp]
        public void setup()
        {
            _headers = new NameValueCollection();
            _oAuthSession = MockRepository.GenerateMock<IOAuthSession>();
            _oAuthContext = MockRepository.GenerateMock<IOAuthContext>();

            _oAuthContext.Expect(x => x.Headers).Return(_headers);

            _consumerRequest = new ConsumerRequest(_oAuthSession, _oAuthContext, null, null);

        }

        [Test]
        public void validation_should_check_for_valid_if_modified_since_header()
        {
            var ifModifiedSince = DateTime.MinValue;
            
            _headers.Add("If-Modified-Since",ifModifiedSince.ToString());
            
            try
            {
                _consumerRequest.Validate();                    
            }
            catch(Exception ex)
            {
                Assert.That(ex, Is.TypeOf(typeof(Exception)));
                Assert.That(ex.Message, Is.EqualTo(string.Format("Supplied value of If-Modified-Since header is too small: {0}", ifModifiedSince.ToString())));
            }

            _consumerRequest.Context.Headers.Clear();
            _headers.Add("If-Modified-Since", "01-Jan-1753");
            _consumerRequest.Validate();                    
        }
        
        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "Supplied value of If-Modified-Since header is too small: 1/01/0001 12:00:00 AM")]
        public void to_consumer_response_should_call_validation()
        {
            var ifModifiedSince = DateTime.MinValue;
            _headers.Add("If-Modified-Since",ifModifiedSince.ToString());
            _consumerRequest.ToConsumerResponse();                    
        }
        
        [TestCase("")]
        [TestCase(null)]
        [TestCase("invalid date time")]
        public void validation_should_ignore_invalid_if_modified_since_header(string ifModifiedSince)
        {
            _headers.Add("If-Modified-Since",ifModifiedSince);
            _consumerRequest.Validate();                    
        } 
    }
}
