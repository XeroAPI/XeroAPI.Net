using System;
using System.Security.Cryptography.X509Certificates;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Storage.Basic;

namespace XeroApi.OAuth
{
    public class XeroApiPartnerSession : OAuthSession
    {
        [Obsolete("Use the constructor with ITokenRepository")]
        public XeroApiPartnerSession(string userAgent, string consumerKey, X509Certificate2 signingCertificate, X509Certificate2 sslCertificate)
            : base(CreateConsumerContext(userAgent, consumerKey, signingCertificate))
        {
            ConsumerRequestFactory = new DefaultConsumerRequestFactory(new SimpleCertificateFactory(sslCertificate));
        }


        public XeroApiPartnerSession(string userAgent, string consumerKey, X509Certificate2 signingCertificate, X509Certificate2 sslCertificate, ITokenRepository tokenRepository)
            : base(CreateConsumerContext(userAgent, consumerKey, signingCertificate), tokenRepository)
        {
            ConsumerRequestFactory = new DefaultConsumerRequestFactory(new SimpleCertificateFactory(sslCertificate));
        }


        private static IOAuthConsumerContext CreateConsumerContext(string userAgent, string consumerKey, X509Certificate2 signingCertificate)
        {
            return new OAuthConsumerContext
            {
                // Public apps use HMAC-SHA1
                SignatureMethod = SignatureMethod.RsaSha1,
                UseHeaderForOAuthParameters = true,

                // Urls
                RequestTokenUri = XeroApiEndpoints.PartnerRequestTokenUri,
                UserAuthorizeUri = XeroApiEndpoints.UserAuthorizeUri,
                AccessTokenUri = XeroApiEndpoints.PartnerAccessTokenUri,
                BaseEndpointUri = XeroApiEndpoints.PartnerBaseEndpointUri,
                
                Key = signingCertificate.PrivateKey,
                ConsumerKey = consumerKey,
                ConsumerSecret = string.Empty,
                UserAgent = userAgent,
            };
        }
    }
}
