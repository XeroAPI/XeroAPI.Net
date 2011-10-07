using System;

namespace DevDefined.OAuth.Storage.Basic
{
    public class FixedValueTokenRepository : ITokenRepository
    {
        private readonly AccessToken _accessToken;
        private readonly RequestToken _requestToken;

        public FixedValueTokenRepository(string requestToken, string requestTokenSecret, string accessToken, string accessTokenSecret)
        {
            _requestToken = new RequestToken { Token = requestToken, TokenSecret = requestTokenSecret };
            _accessToken = new AccessToken { Token = accessToken, TokenSecret = accessTokenSecret };
        }

        public RequestToken GetRequestToken()
        {
            return _requestToken;
        }

        public void SaveRequestToken(RequestToken requestToken)
        {
            throw new NotImplementedException("The request token cannot be altered when using the FixedValueTokenRepository");
        }

        public AccessToken GetAccessToken()
        {
            return _accessToken;
        }

        public void SaveAccessToken(AccessToken accessToken)
        {
            throw new NotImplementedException("The access token cannot be altered when using the FixedValueTokenRepository");
        }
    }
}
