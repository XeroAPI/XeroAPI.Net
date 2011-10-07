namespace DevDefined.OAuth.Storage.Basic
{
    /// <summary>
    /// In-Memory implementation of a token repository
    /// </summary>
    public class InMemoryTokenRepository : ITokenRepository
    {
        private RequestToken _requestToken;
        private AccessToken _accessToken;

        public RequestToken GetRequestToken()
        {
            return _requestToken;
        }

        public void SaveRequestToken(RequestToken requestToken)
        {
            _requestToken = requestToken;
        }

        public AccessToken GetAccessToken()
        {
            return _accessToken;
        }

        public void SaveAccessToken(AccessToken accessToken)
        {
            _accessToken = accessToken;
        }
    }
}