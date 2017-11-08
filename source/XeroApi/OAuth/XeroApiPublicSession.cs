using System;
using Booyami.DevDefined.OAuth.Consumer;
using Booyami.DevDefined.OAuth.Framework;
using Booyami.DevDefined.OAuth.Storage.Basic;

namespace XeroApi.OAuth
{
    public class XeroApiPublicSession : OAuthSession   
    {
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
