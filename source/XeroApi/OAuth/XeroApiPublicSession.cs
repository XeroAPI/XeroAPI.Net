using System;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;

namespace XeroApi.OAuth
{
    public class XeroApiPublicSession : OAuthSession   
    {
        public XeroApiPublicSession(string userAgent, string consumerKey, string consumerSecret)
            : base(CreateConsumerContext(userAgent, consumerKey, consumerSecret))
        {
        }

        public XeroApiPublicSession(string userAgent, string consumerKey, string consumerSecret, string accessToken, string accessSecret)
            : base(CreateConsumerContext(userAgent, consumerKey, consumerSecret))
        {
            AccessToken = new TokenBase {Token = accessToken, TokenSecret = accessSecret};
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
