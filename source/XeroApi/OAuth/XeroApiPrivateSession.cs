using System.Security.Cryptography.X509Certificates;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Storage.Basic;

namespace XeroApi.OAuth
{
    public class XeroApiPrivateSession : OAuthSession
    {
        public XeroApiPrivateSession(string userAgent, string consumerKey, X509Certificate2 signingCertificate) 
            : base(
                CreateConsumerContext(userAgent, consumerKey, signingCertificate), 
                new FixedValueTokenRepository(consumerKey, string.Empty, consumerKey, string.Empty)
            )
        {
        }
        

        private static IOAuthConsumerContext CreateConsumerContext(string userAgent, string consumerKey, X509Certificate2 signingCertificate)
        {
            return new OAuthConsumerContext
            {
                // Public apps use HMAC-SHA1
                SignatureMethod = SignatureMethod.RsaSha1,
                UseHeaderForOAuthParameters = true,

                // Urls
                RequestTokenUri = null,
                UserAuthorizeUri = null,
                AccessTokenUri = XeroApiEndpoints.PublicAccessTokenUri,
                BaseEndpointUri = XeroApiEndpoints.PublicBaseEndpointUri,

                Key = signingCertificate.PrivateKey,
                ConsumerKey = consumerKey,
                ConsumerSecret = string.Empty,
                UserAgent = userAgent,
            };
        }
        
        
    }
}
