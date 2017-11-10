using System;
using System.Collections.Specialized;
using System.Net;
using Booyami.DevDefined.OAuth.Consumer;
using Booyami.DevDefined.OAuth.Framework;
using Booyami.DevDefined.OAuth.Storage.Basic;
using NUnit.Framework;

using Rhino.Mocks;

namespace XeroApi.Tests.OAuth.Consumer 
{
    [TestFixture]
    public class ConsumerRequestTests
    {
        [Test]
        public void to_consumer_response_with_really_old_date_string_should_fail_validation()
        {
            const string reallyOldDateString = "1000-01-01T00:00:00";

            var oAuthContext = new OAuthContext
            {
                RequestMethod = "GET", 
                RawUri = new Uri("http://any.uri/"),
                Headers = new WebHeaderCollection { { "If-Modified-Since", reallyOldDateString } }
            };

            var consumerRequest = NewConsumerRequest(oAuthContext);

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => consumerRequest.ToWebRequest());

            const string expectedMessage = "Supplied value of If-Modified-Since header is too small";
            Assert.IsTrue(ex.Message.Contains(expectedMessage), string.Format("Exception message <{0}> is not as expected <{1}>", ex.Message, expectedMessage));
        }

        [Test]
        public void to_consumer_response_with_current_date_string_should_pass_validation()
        {
            const string recentDateString = "2012-04-01T00:00:00";

            var oAuthContext = new OAuthContext
            {
                RequestMethod = "GET",
                RawUri = new Uri("http://any.uri/"),
                Headers = new WebHeaderCollection { { "If-Modified-Since", recentDateString } }
            };

            var consumerRequest = NewConsumerRequest(oAuthContext);

            Assert.DoesNotThrow(() => consumerRequest.ToWebRequest());
        }
        
        [Test]
        public void it_can_parse_unset_IfModifiedSince_date()
        {
            ConsumerRequest consumerRequest = new ConsumerRequest(null, null, null, null);
            NameValueCollection headers = new NameValueCollection();
            OAuthContext oauthContext = new OAuthContext {Headers = headers};

            Assert.AreEqual(null, consumerRequest.ParseIfModifiedSince(oauthContext));
        }

        [Test]
        public void it_can_parse_empty_IfModifiedSince_date()
        {
            var consumerRequest = NewConsumerRequest();
            var headers = new NameValueCollection { {"If-Modified-Since", ""}};
            var oauthContext = new OAuthContext { Headers = headers };

            Assert.AreEqual(null, consumerRequest.ParseIfModifiedSince(oauthContext));
        }

        [Test]
        public void it_can_parse_null_IfModifiedSince_date()
        {
            var consumerRequest = NewConsumerRequest();
            var headers = new NameValueCollection { { "If-Modified-Since", null } };
            var oauthContext = new OAuthContext { Headers = headers };

            Assert.AreEqual(null, consumerRequest.ParseIfModifiedSince(oauthContext));
        }

        [Test]
        public void it_can_parse_valid_IfModifiedSince_date()
        {
            var consumerRequest = NewConsumerRequest();
            var headers = new NameValueCollection { { "If-Modified-Since", "2012-04-01 23:45:00" } };
            var oauthContext = new OAuthContext { Headers = headers };

            Assert.AreEqual(new DateTime(2012, 04, 01, 23, 45, 0), consumerRequest.ParseIfModifiedSince(oauthContext));
        }

        [Test]
        public void it_can_parse_too_early_IfModifiedSince_date()
        {
            var consumerRequest = NewConsumerRequest();
            var headers = new NameValueCollection { { "If-Modified-Since", "1752-12-31 23:59:59" } };
            var oauthContext = new OAuthContext { Headers = headers };

            Assert.Throws<ArgumentOutOfRangeException>(() => consumerRequest.ParseIfModifiedSince(oauthContext));
        }

        [Test]
        public void it_can_parse_very_early_IfModifiedSince_date()
        {
            var consumerRequest = NewConsumerRequest();
            var headers = new NameValueCollection { { "If-Modified-Since", "1753-01-01 00:00:00" } };
            var oauthContext = new OAuthContext { Headers = headers };

            Assert.AreEqual(new DateTime(1753, 01, 01, 0, 0, 0), consumerRequest.ParseIfModifiedSince(oauthContext));
        }

        [Test]
        public void it_can_read_IfModifiedSince_date_from_oauth_context_property()
        {
            var consumerRequest = NewConsumerRequest();

            var oauthContext = new OAuthContext
            {
                Headers = new NameValueCollection(), 
                IfModifiedSince = new DateTime(1753,01,02,3,4,5)
            };

            Assert.AreEqual(new DateTime(1753, 01, 02, 3, 4, 5), consumerRequest.ParseIfModifiedSince(oauthContext));
        }

        private static ConsumerRequest NewConsumerRequest()
        {
            return new ConsumerRequest(null, null, null, null);
        }

        private static ConsumerRequest NewConsumerRequest(OAuthContext oAuthContext)
        {
            var consumerRequestRunner = MockRepository.GenerateMock<IConsumerRequestRunner>();

            var consumerContext = new OAuthConsumerContext { ConsumerKey = "xxx", SignatureMethod = "HMAC-SHA1" };

            var tokenRepository = new InMemoryTokenRepository();

            var oAuthSession = new OAuthSession(consumerContext, tokenRepository)
            {
                ConsumerRequestRunner = consumerRequestRunner
            };

            return new ConsumerRequest(oAuthSession, oAuthContext, consumerContext, new NullCertificateFactory());
        }
    }
}
