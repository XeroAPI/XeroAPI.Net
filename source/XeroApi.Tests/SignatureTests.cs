using System;
using System.Security.Cryptography.X509Certificates;
using Booyami.DevDefined.OAuth.Consumer;
using Booyami.DevDefined.OAuth.Framework;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using XeroApi.OAuth;

namespace XeroApi.Tests
{
	[TestFixture]
    public class SignatureTests
    {
        private const string UserAgent = "Signature Testing";
        private const string ConsumerKey = "dpf43f3p2l4k3l03";

        [Test]
        public void TestSignRequestToken()
        {
            var cert = new X509Certificate2("XeroApiNet-Sample.pfx", "password");

            var consumerContext = new OAuthConsumerContext
            {
                // Partner and Private apps use RSA-SHA1 signing method
                SignatureMethod = SignatureMethod.RsaSha1,
                UseHeaderForOAuthParameters = true,

                // Urls
                RequestTokenUri = null,
                UserAuthorizeUri = null,
                AccessTokenUri = XeroApiEndpoints.PublicAccessTokenUri,
                BaseEndpointUri = XeroApiEndpoints.PublicBaseEndpointUri,

                Key = cert.PrivateKey,
                ConsumerKey = ConsumerKey,
                ConsumerSecret = string.Empty,
                UserAgent = UserAgent,
            };

            var oauthContext = new OAuthContext
            {
                RequestMethod = "POST",
                RawUri = new Uri("https://photos.example.net/request_token"),
                ConsumerKey = "dpf43f3p2l4k3l03",
                SignatureMethod = SignatureMethod.RsaSha1,
                Timestamp = "1191242090",
                Nonce = "hsu94j3884jdopsl",
                Version = "1.0"
            };

            var signatureBase = oauthContext.GenerateSignatureBase();
            consumerContext.Signer.SignContext(oauthContext,
                                               new SigningContext
                                               {
                                                   Algorithm = consumerContext.Key,
                                                   ConsumerSecret = null,
                                                   SignatureBase = signatureBase
                                               });

            Assert.That(oauthContext.Signature, Is.EqualTo("aIIAFPjD0uavubFeL/Hz4LSV6NsvAbrvfnPF6OcgGfhML5ezO0+E+tofLgp1SHbLyNFM7D1p/SJN1J4MY7T3HzvM8HX+3u5Q+Ui+en0/ewHZ+3ar6BA7r3zOYqDn8rfCGSnweia3fFYmjkeS8NvKShnewUu0jUFbnG4RXw8BiEk="));
        }

        [Test]
        public void TestSignAccessToken()
        {
            var cert = new X509Certificate2("XeroApiNet-Sample.pfx", "password");
            
            var consumerContext = new OAuthConsumerContext
            {
                // Partner and Private apps use RSA-SHA1 signing method
                SignatureMethod = SignatureMethod.RsaSha1,
                UseHeaderForOAuthParameters = true,

                // Urls
                RequestTokenUri = null,
                UserAuthorizeUri = null,
                AccessTokenUri = XeroApiEndpoints.PublicAccessTokenUri,
                BaseEndpointUri = XeroApiEndpoints.PublicBaseEndpointUri,

                Key = cert.PrivateKey,
                ConsumerKey = ConsumerKey,
                ConsumerSecret = string.Empty,
                UserAgent = UserAgent,
            };

            var oauthContext = new OAuthContext
                {
                    RequestMethod = "POST",
                    RawUri = new Uri("https://photos.example.net/access_token"),
                    ConsumerKey = "dpf43f3p2l4k3l03",
                    SignatureMethod = SignatureMethod.RsaSha1,
                    Timestamp = "1191242090",
                    Token = "hh5s93j4hdidpola",
                    TokenSecret = "hdhd0244k9j7ao03",
                    Nonce = "hsu94j3884jdopsl",
                    Verifier = "hfdp7dh39dks9884",
                    Version = "1.0"
                };

            var signatureBase = oauthContext.GenerateSignatureBase();
            consumerContext.Signer.SignContext(oauthContext,
                                               new SigningContext
                                                   {
                                                       Algorithm = consumerContext.Key,
                                                       ConsumerSecret = null,
                                                       SignatureBase = signatureBase
                                                   });

            Assert.That(oauthContext.Signature, Is.EqualTo("32vGleSAIeMbgW9E0pC+PUkyZ1Y05zuEd+FZwg+w4jZzj3E1zldbrGY5SnVpypZfjixWuHMtV4mwGwptwiTZRkrLBudWqJDEddvlwuIMY1j6WkQulz/IXzbGuPNgTya/KTEhQ5IExJXCKE1LZ9bNsMXBDpyi7/ayZe9ONqoVzS8="));
        }
    }
}
