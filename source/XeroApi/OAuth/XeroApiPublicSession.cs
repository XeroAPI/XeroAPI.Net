using System;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Storage.Basic;

namespace XeroApi.OAuth
{
    public class XeroApiPublicSession : OAuthSession   
    {
        [Obsolete("Use the constructor with ITokenRepository")]
        public XeroApiPublicSession(string userAgent, string consumerKey, string consumerSecret)
            : base(CreateConsumerContext(userAgent, consumerKey, consumerSecret))
        {
        }


        public XeroApiPublicSession(string userAgent, string consumerKey, string consumerSecret, ITokenRepository tokenRepository)
            : base(CreateConsumerContext(userAgent, consumerKey, consumerSecret), tokenRepository)
        {
        }


        private static IOAuthConsumerContext CreateConsumerContext(string userAgent, string consumerKey, string consumerSecret)
        {
            return new OAuthConsumerContext
            {
                // Public apps use HMAC-SHA1
                SignatureMethod = SignatureMethod.HmacSha1,
                UseHeaderForOAuthParameters = true,

                // Urls
                RequestTokenUri = XeroApiEndpoints.PublicRequestTokenUri,
                UserAuthorizeUri = XeroApiEndpoints.UserAuthorizeUri,
                AccessTokenUri = XeroApiEndpoints.PublicAccessTokenUri,
                BaseEndpointUri = XeroApiEndpoints.PublicBaseEndpointUri,
                
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                UserAgent = userAgent,
            };
        }
    }
}
