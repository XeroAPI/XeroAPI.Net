using System;
using System.Security.Cryptography.X509Certificates;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
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
            var cert = new X509Certificate2("test-cert.pfx");

            var consumerContext = new OAuthConsumerContext
            {
                // Public apps use HMAC-SHA1
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
            Assert.That(oauthContext.Signature, Is.EqualTo("YpkhJYmKjwAprqQ0JO4sSkpo09F3kc/D12dRoDmi5q/S096krV0B1PpZl5Rb8acP9yvileXFMQaU4lvOya1PJ2g9wUMfewOwRn3Ua7Uudk7VXpaFJhTenktWBEh+2YjxUPEkD3vFPdc+R/n5FEHzYSyQ6b270vrTh+4nyuPUz5RKBzdiccKMfcsEMMrN097Nmpz+Tt6Zpbv2zvxz/TYPT3lfi7CKTtpqD3WSPD+nyAXc+n0n8xgqZdQ+BcoVWcIxUNKZHmxmDhAWoPrMpZmO1krRy1JPq8eHPrLWn0Owqw2LAcPCEmLzF/lwrBCIbIAJcTIoEYMycmM2wE46x9L2ew=="));
        }

        [Test]
        public void TestSignAccessToken()
        {
            var cert = new X509Certificate2("test-cert.pfx");
            
            var consumerContext = new OAuthConsumerContext
            {
                // Public apps use HMAC-SHA1
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
            Assert.That(oauthContext.Signature, Is.EqualTo("HSrsfpD8CTgov5d09skqoIo7ovj3tQrvYHpQ/HwrlbTGBcJy7S4Vu4vnGcbrnAZGCUL1+loKIpvQY/Fj72VtVhnuBirDfqmdbSTQNYgDDELmUhacVqLhLoysMNAs9WWWNpmaZkgD7cKbtdLJ6+oMCsGqUGHj1rUqb37fqfgYNkajj47Ai0y1FT3+BaeGXf5d68o56UIIK3jcq1ibCdORd7S0onxPG95cqo4bTxrPejxqJdZGGtYg6q3MlQGEBKm4qVbRjoITTz5VgoIz9sIDYfX9/GWVxk2y6wc3F+D7Ue6RPc3KyorSLqwa92tQ0rXhLnmhHWoC5BcnDB0oYPlJaw=="));
        }
    }
}
